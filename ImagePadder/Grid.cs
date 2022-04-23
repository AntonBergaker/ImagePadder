using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImagePadder {

    /// <summary>
    /// Because it seems every task needs a grid
    /// </summary>
    public class Grid<T> : IEnumerable<T>, ICloneable {
        private readonly T[,] data;
        public readonly int Width;
        public readonly int Height;

        public T this[int x, int y] {
            get => data[x, y];
            set => data[x, y] = value;
        }

        public Grid(int width, int height) {
            data = new T[width, height];
            Width = width;
            Height = height;
        }

        public Grid(T[,] data) {
            this.data = data;
            Width = data.GetLength(0);
            Height = data.GetLength(1);
        }

        public IEnumerator<T> GetEnumerator() {
            return data.Cast<T>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public Grid<T> Clone() {
            Grid<T> clone = new Grid<T>(Width, Height);
            Array.Copy(data, clone.data, data.Length);
            return clone;
        }

        internal T[,] To2DArray() {
            return data;
        }

        public string ToGridString() {
            return ToGridString(x => x.ToString());
        }

        public string ToGridString(Func<T, string> toStringFunc) {
            StringBuilder sb = new StringBuilder();
            for (int y = 0; y < Height; y++) {
                if (y > 0) {
                    sb.Append("\n");
                }
                for (int x = 0; x < Width; x++) {
                    sb.Append(toStringFunc(data[x, y]));
                }
            }

            return sb.ToString();
        }

        object ICloneable.Clone() {
            return this.Clone();
        }

        public void SetAll(T value) {
            for (int x = 0; x < Width; x++) {
                for (int y = 0; y < Height; y++) {
                    data[x, y] = value;
                }
            }
        }
    }
}