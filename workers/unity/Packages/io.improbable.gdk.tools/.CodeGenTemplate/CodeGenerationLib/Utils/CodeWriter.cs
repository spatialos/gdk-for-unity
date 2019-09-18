using System;
using System.Text;

namespace Improbable.Gdk.CodeGeneration.Utils
{
    public class CodeWriter
    {
        private int scopeCounter;
        private bool hasBuiltAlready;
        private StringBuilder builder;

        private const int TabLength = 4;
        private string currentIndent = "";

        public CodeWriter()
        {
            builder = new StringBuilder();
        }

        public CodeScope Scope(string prefix = null)
        {
            return new CodeScope(this, prefix);
        }

        public void WriteLine(string line)
        {
            Indent();
            builder.AppendLine(line);
        }

        public string Build()
        {
            if (scopeCounter != 0)
            {
                throw new InvalidOperationException("Cannot build when scope is not at the root scope.");
            }

            if (hasBuiltAlready)
            {
                throw new InvalidOperationException("Cannot call Finish() more than once on the same CodeWriter instance");
            }

            hasBuiltAlready = true;
            return builder.ToString();
        }

        private void Indent()
        {
            builder.Append(currentIndent);
        }

        private void UpdateCurrentIndent()
        {
            currentIndent = new String(' ', TabLength * scopeCounter);
        }

        public struct CodeScope : IDisposable
        {
            private CodeWriter writer;

            public CodeScope(CodeWriter writer, string prefix = null)
            {
                if (writer == null)
                {
                    throw new ArgumentException("Cannot pass a null CodeWriter into a CodeScope.");
                }

                this.writer = writer;

                if (!string.IsNullOrEmpty(prefix))
                {
                    writer.WriteLine(prefix);
                }
                writer.WriteLine("{");
                writer.scopeCounter++;
                writer.UpdateCurrentIndent();
            }

            public void Dispose()
            {
                if (writer == null)
                {
                    throw new InvalidOperationException("Cannot Dispose a CodeScope more than once");
                }

                writer.scopeCounter--;
                writer.UpdateCurrentIndent();
                writer.WriteLine("}");

                writer = null;
            }
        }
    }
}
