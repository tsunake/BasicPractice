using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple.Common
{
    public class Sort
    {
        #region merge sort
        public static void MergeSort<T>(List<T> src) where T : IComparable<T>
        {
            List<T> tmp = new List<T>(src.Count);
            tmp.AddRange(Enumerable.Repeat<T>(default(T), src.Count));

            MergeSort<T>(src, tmp, 0, src.Count - 1);
        }

        private static void MergeSort<T>(List<T> src, List<T> tmp, int left, int right) where T : IComparable<T>
        {
            if (left >= right)
                return;
            int mid = (left + right) / 2;
            MergeSort<T>(src, tmp, left, mid);
            MergeSort<T>(src, tmp, mid + 1, right);
            Merge<T>(src, tmp, left, mid + 1, right);
        }

        private static void Merge<T>(List<T> src, List<T> tmp, int left_begin, int right_begin, int right_end) where T : IComparable<T>
        {
            int left_end = right_begin - 1;
            int tmp_pos = left_begin;
            int count = right_end - left_begin + 1;
            while (left_begin <= left_end && right_begin <= right_end)
            {
                if (src[left_begin].CompareTo(src[right_begin]) < 0)
                    tmp[tmp_pos++] = src[left_begin++];
                else
                    tmp[tmp_pos++] = src[right_begin++];
            }
            while (left_begin <= left_end)
                tmp[tmp_pos++] = src[left_begin++];
            while (right_begin <= right_end)
                tmp[tmp_pos++] = src[right_begin++];
            for (int i = 0; i < count; ++i, right_end--)
                src[right_end] = tmp[right_end];
        }
        #endregion

        #region quick sort
        public static void QuickSort<T>(List<T> src) where T : IComparable<T>
        {
            QuickSort<T>(src, 0, src.Count - 1);
        }

        private static void QuickSort<T>(List<T> src, int left, int right) where T : IComparable<T>
        {
            if (left >= right)
                return;
            int low = left;
            int high = right;
            T pivot = src[left];
            while(low < high)
            {
                while (high > low && src[high].CompareTo(pivot) >= 0)
                    --high;
                src[low] = src[high];
                while (low < high && src[low].CompareTo(pivot) <= 0)
                    ++low;
                src[high] = src[low];
            }
            src[low] = pivot;
            QuickSort<T>(src, left, low - 1);
            QuickSort<T>(src, low + 1, right);
        }
        #endregion
    }
}
