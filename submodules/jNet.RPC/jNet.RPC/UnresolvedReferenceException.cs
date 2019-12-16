﻿using System;

namespace jNet.RPC
{
    public class UnresolvedReferenceException: Exception
    {
        public Guid Guid { get; }

        public UnresolvedReferenceException(Guid guid)
        {
            Guid = guid;
        }
    }
}
