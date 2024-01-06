﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace PatataStudio.Utils
{
	/// <summary>
	/// An array usable as both a NativeArray and managed array
	/// </summary>
	/// <typeparam name="T">The type of the array element</typeparam>
	public class SharedArray<T> : SharedArray<T, T> where T : unmanaged
	{
		public SharedArray(T[] managed) { Initialize(managed); }
		public SharedArray(int size) { Initialize(new T[size]); }
	}

	/// <summary>
	/// An array usable as both a NativeArray and Managed array
	/// </summary>
	/// <typeparam name="T">The element type in the managed representation</typeparam>
	/// <typeparam name="TNative"> The element type in the NativeArray representation. Must be the same size as T</typeparam>
	public class SharedArray<T, TNative> : IDisposable, IEnumerable<T>
		where T : unmanaged
		where TNative : unmanaged
	{
		protected GCHandle m_GcHandle;
#if UNITY_EDITOR
		protected AtomicSafetyHandle m_SafetyHandle;
#endif
		protected T[] m_Managed;
		protected NativeArray<TNative> m_Native;

		public int Length => m_Managed.Length;

		public SharedArray(T[] managed)
		{
			ThrowIfTypesNotEqualSize();
			Initialize(managed);
		}

		public SharedArray(int size)
		{
			ThrowIfTypesNotEqualSize();
			Initialize(new T[size]);
		}

		protected SharedArray() { }

		public static implicit operator NativeArray<TNative>(SharedArray<T, TNative> self) => self.m_Native;

		public static implicit operator T[](SharedArray<T, TNative> self)
		{
#if UNITY_EDITOR && !DISABLE_SHAREDARRAY_SAFETY
			AtomicSafetyHandle.CheckWriteAndThrow(self.m_SafetyHandle);
#endif
			return self.m_Managed;
		}

		protected void Initialize(T[] managed)
		{
			m_Managed = managed;
			Initialize();
		}

		void Initialize()
		{
			m_GcHandle = GCHandle.Alloc(m_Managed, GCHandleType.Pinned);
			CreateNativeAlias();
		}

		protected unsafe void CreateNativeAlias()
		{
			fixed (void* ptr = m_Managed)
			{
				m_Native = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<TNative>
					(ptr, m_Managed.Length, Allocator.None);
			}

#if UNITY_EDITOR
			m_SafetyHandle = AtomicSafetyHandle.Create();
			NativeArrayUnsafeUtility.SetAtomicSafetyHandle(ref m_Native, m_SafetyHandle);
#endif
		}

		public ref T GetPinnableReference() => ref m_Managed[0];

		public void Resize(int newSize)
		{
			if (newSize == m_Managed.Length)
				return;

#if UNITY_EDITOR
			AtomicSafetyHandle.CheckDeallocateAndThrow(m_SafetyHandle);
			AtomicSafetyHandle.Release(m_SafetyHandle);
#endif
			if (m_GcHandle.IsAllocated) m_GcHandle.Free();

			Array.Resize(ref m_Managed, newSize);
			Initialize();
		}

		public void Clear()
		{
#if UNITY_EDITOR
			AtomicSafetyHandle.CheckWriteAndThrow(m_SafetyHandle);
#endif
			Array.Clear(m_Managed, 0, m_Managed.Length);
		}

		public IEnumerator<T> GetEnumerator()
		{
#if UNITY_EDITOR
			AtomicSafetyHandle.CheckReadAndThrow(m_SafetyHandle);
#endif
			return ((IEnumerable<T>)m_Managed).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public void Dispose()
		{
			if (m_Managed == null)
				return;
#if UNITY_EDITOR
			AtomicSafetyHandle.CheckDeallocateAndThrow(m_SafetyHandle);
			AtomicSafetyHandle.Release(m_SafetyHandle);
#endif
			m_Managed = null;
			if (m_GcHandle.IsAllocated) m_GcHandle.Free();
		}

		~SharedArray() { Dispose(); }

		static unsafe void ThrowIfTypesNotEqualSize()
		{
			if (sizeof(T) != sizeof(TNative))
			{
				throw new InvalidOperationException(
					$"Size of native alias type '{typeof(TNative).FullName}' ({sizeof(TNative)} bytes) " +
					$"must be equal to size of source type '{typeof(T).FullName}' ({sizeof(T)} bytes)");
			}
		}
	}
}