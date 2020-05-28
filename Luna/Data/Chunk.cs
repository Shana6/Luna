﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Luna {
    class Chunk : IDisposable {
        #region Handlers
        public delegate void Handler(Game _game, BinaryReader _reader, BinaryWriter _writer, Chunk _chunk);
        public static Dictionary<string, Handler> Handlers = new Dictionary<string, Handler>() {
            { "STRG", ChunkHandler.STRG },
            { "GEN8", ChunkHandler.GEN8 },
            { "ROOM", ChunkHandler.ROOM },
            { "VARI", ChunkHandler.VARI },
            { "FUNC", ChunkHandler.FUNC },
            { "CODE", ChunkHandler.CODE }
        };

        #endregion

        public string Name;
        public Int32 Length;
        public long Base;

        public Chunk(BinaryReader _reader) {
            this.Name = ASCIIEncoding.ASCII.GetString(_reader.ReadBytes(4));
            this.Length = _reader.ReadInt32();
            this.Base = _reader.BaseStream.Position;
#if (DEBUG == true)
            Console.WriteLine(this);
#endif
        }

        public void Dispose() { }

        public override string ToString() {
            return $"Chunk: {this.Name}, Length: {this.Length} bytes";
        }
    }
}
