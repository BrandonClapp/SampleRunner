using System;
using FluentMigrator.Runner.Processors.DotConnectOracle;

namespace ConsoleApp1.Core
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class SequenceAttribute : Attribute
    {
        public int Sequence { get; }
        
        public SequenceAttribute(int sequence)
        {
            this.Sequence = sequence;
        }
    }
}