﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luna.Assets;
using System.IO;

namespace Luna.Types {
    class LScript {
        public string Name;
        public Int32 Index;

        public LScript(Game _game, BinaryReader _reader) {
            this.Name = _game.GetString(_reader.ReadInt32());
            this.Index = _reader.ReadInt32();
        }
    }
}
