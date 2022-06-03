using System;
using System.Collections.Generic;
using System.Text;

namespace Simple1C77
{
    public class Token
    {
        public string type;
        public string value;
        (uint, uint) position;

        public Token(string type, string value, (uint, uint) position = default((uint, uint)))
        {
            this.type = type;
            this.value = value;
            this.position = position;
        }

        public override string ToString()
        {
            return string.Format("Token({0}, {1}, {2})",
                                type,
                                value,
                                position);
        }

        public void RaiseError(string description = "") 
        {
            throw new ArgumentException( this.ToString() + " " + description, "ProgramText");
        }

    }
}
