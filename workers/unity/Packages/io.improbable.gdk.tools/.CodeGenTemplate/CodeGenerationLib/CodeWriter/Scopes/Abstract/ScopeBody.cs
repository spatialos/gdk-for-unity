using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Improbable.Gdk.CodeGeneration.CodeWriter.Scopes
{
    /// <summary>
    /// A scoped unit of code with helpers for writing content found in method bodies.
    /// </summary>
    public abstract class ScopeBody : ScopeBlock
    {
        protected ScopeBody(string declaration, Action<ScopeBody> populate) : base(declaration)
        {
            populate(this);
        }

        protected ScopeBody(string declaration = null) : base(declaration)
        {
        }

        public void Text(Text text)
        {
            Add(text);
        }

        public void TextList(TextList textList)
        {
            Add(textList);
        }

        public void Line(string snippet)
        {
            Add(new Text(snippet));
        }

        public void Line(Action<StringBuilder> populate)
        {
            var sb = new StringBuilder();
            populate(sb);

            Add(new Text(sb.ToString()));
        }

        public void Line(ICollection<string> snippets)
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

        public void CustomScope(Func<IEnumerable<string>> populate)
        {
            Add(new CustomScopeBlock(populate));
        }

        public void CustomScope(string declaration, Action<CustomScopeBlock> populate)
        {
            Add(new CustomScopeBlock(declaration, populate));
        }

        public void CustomScope(string declaration, Func<IEnumerable<string>> populate)
        {
            Add(new CustomScopeBlock(declaration, populate));
        }

        public void Loop(string declaration, Action<LoopBlock> populate)
        {
            Add(new LoopBlock(declaration, populate));
        }

        public void Loop(string declaration, Func<IEnumerable<string>> populate)
        {
            Add(new LoopBlock(declaration, populate));
        }

        public IfElseBlock If(string declaration, Action<ScopeBody> populate)
        {
            var ifElseBlock = new IfElseBlock(declaration, populate);
            Add(ifElseBlock);
            return ifElseBlock;
        }

        public IfElseBlock If(string declaration, Func<IEnumerable<string>> populate)
        {
            var ifElseBlock = new IfElseBlock(declaration, populate);
            Add(ifElseBlock);
            return ifElseBlock;
        }

        public TryCatchBlock Try(Action<ScopeBody> populate)
        {
            var tryCatchBlock = new TryCatchBlock(populate);
            Add(tryCatchBlock);
            return tryCatchBlock;
        }

        public TryCatchBlock Try(Func<IEnumerable<string>> populate)
        {
            var tryCatchBlock = new TryCatchBlock(populate);
            Add(tryCatchBlock);
            return tryCatchBlock;
        }

        public void Return()
        {
            Line("return;");
        }

        public void Return(string toReturn)
        {
            Line($"return {toReturn};");
        }

        public void ProfileScope(string markerVariableName, Action<CustomScopeBlock> populate)
        {
            Add(new CustomScopeBlock($"using ({markerVariableName}.Auto())", populate));
        }
    }
}
