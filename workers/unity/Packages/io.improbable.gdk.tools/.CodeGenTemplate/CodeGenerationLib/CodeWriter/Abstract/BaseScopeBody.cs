using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Improbable.Gdk.CodeGeneration.CodeWriter.NoScope;

namespace Improbable.Gdk.CodeGeneration.CodeWriter.Scopes
{
    public abstract class BaseScopeBody : BaseScopeBlock
    {
        protected BaseScopeBody(string declaration, Action<BaseScopeBody> populate) : base(declaration)
        {
            populate(this);
        }

        protected BaseScopeBody(string declaration = null) : base(declaration)
        {
        }

        public void WriteLine(string snippet)
        {
            Add(new TextBlock(snippet));
        }

        public void WriteLine(Action<StringBuilder> populate)
        {
            var sb = new StringBuilder();
            populate(sb);

            Add(new TextBlock(sb.ToString()));
        }

        public void WriteLine(IEnumerable<string> snippets)
        {
            if (snippets.Any())
            {
                Add(new TextList(snippets));
            }
        }

        public void Initializer(string declaration, Func<IEnumerable<string>> populate)
        {
            Add(new InitializerBlock(declaration, populate));
        }

        public void CustomScope(Action<CustomScopeBlock> populate)
        {
            Add(new CustomScopeBlock(populate));
        }

        public void CustomScope(string declaration, Action<CustomScopeBlock> populate)
        {
            Add(new CustomScopeBlock(declaration, populate));
        }

        public void Loop(string declaration, Action<LoopBlock> populate)
        {
            Add(new LoopBlock(declaration, populate));
        }

        public IfElseBlock If(string declaration, Action<BaseScopeBody> populate)
        {
            var ifElseBlock = new IfElseBlock(declaration, populate);
            Add(ifElseBlock);
            return ifElseBlock;
        }

        public TryCatchBlock Try(Action<BaseScopeBody> populate)
        {
            var tryCatchBlock = new TryCatchBlock(populate);
            Add(tryCatchBlock);
            return tryCatchBlock;
        }

        public void Continue()
        {
            WriteLine("continue;");
        }

        public void Break()
        {
            WriteLine("break;");
        }

        public void Return()
        {
            WriteLine("return;");
        }

        public void Return(string toReturn)
        {
            WriteLine($"return {toReturn};");
        }

        public void ProfileStart(string name)
        {
            WriteLine($"Profiler.BeginSample(\"{name}\");");
        }

        public void ProfileEnd()
        {
            WriteLine("Profiler.EndSample();");
        }
    }
}
