using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace HashTablesBoth.Entities
{
    public class Pair
    {
        private int key;
        private int value;
        private bool deleted;
        public Pair(int key, int value)
        {
            this.key = key;
            this.value = value;
            this.deleted = false;
        }
        public int getKey()
        {
            return key;
        }

        public int getValue()
        {
            return value;
        }

        public bool isDeleted()
        {
            return deleted;
        }

        public bool deletePair()
        {
            if (!this.deleted)
            {
                this.deleted = true;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}