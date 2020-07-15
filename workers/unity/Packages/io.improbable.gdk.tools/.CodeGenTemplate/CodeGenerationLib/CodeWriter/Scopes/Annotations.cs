using System;
using System.Collections.Generic;
using Improbable.Gdk.CodeGeneration.CodeWriter.Scopes;

namespace CodeGenerationLib.CodeWriter.Scopes
{
    public class AnnotationOutsideType
    {
        protected readonly ScopeBlock ParentBlock;
        protected readonly List<string> Annotations;

        internal AnnotationOutsideType(ScopeBlock parentBlock, string annotation)
        {
            ParentBlock = parentBlock;
            Annotations = new List<string> { annotation };
        }

        public void Type(string declaration, Action<TypeBlock> populate)
        {
            ParentBlock.Add(new TypeBlock(declaration, populate, Annotations));
        }

        public void Type(TypeBlock typeBlock)
        {
            ParentBlock.Add(typeBlock);
        }

        public void Enum(string declaration, Action<EnumBlock> populate)
        {
            ParentBlock.Add(new EnumBlock(declaration, populate, Annotations));
        }

        public void Enum(EnumBlock enumBlock)
        {
            ParentBlock.Add(enumBlock);
        }

        public AnnotationOutsideType Annotate(string annotation)
        {
            Annotations.Add(annotation);
            return this;
        }
    }

    public class AnnotationInsideType : AnnotationOutsideType
    {
        internal AnnotationInsideType(ScopeBlock parentBlock, string annotation) : base(parentBlock, annotation)
        {
        }

        public void Method(string declaration, Action<MethodBlock> populate)
        {
            ParentBlock.Add(new MethodBlock(declaration, populate, Annotations));
        }

        public void Method(string declaration, Func<IEnumerable<string>> populate)
        {
            ParentBlock.Add(new MethodBlock(declaration, populate, Annotations));
        }

        public void Method(MethodBlock methodBlock)
        {
            ParentBlock.Add(methodBlock);
        }

        public new AnnotationInsideType Annotate(string annotation)
        {
            Annotations.Add(annotation);
            return this;
        }
    }
}
