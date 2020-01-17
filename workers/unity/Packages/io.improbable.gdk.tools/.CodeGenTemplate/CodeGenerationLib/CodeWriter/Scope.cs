using System;
using System.Collections.Generic;
using Improbable.Gdk.CodeGeneration.CodeWriter.Scopes;

namespace Improbable.Gdk.CodeGeneration.CodeWriter
{
    public static class Scope
    {
        public static CustomScopeBlock Custom(Action<CustomScopeBlock> populate)
        {
            return new CustomScopeBlock(populate);
        }

        public static CustomScopeBlock Custom(Func<IEnumerable<string>> populate)
        {
            return new CustomScopeBlock(populate);
        }

        public static CustomScopeBlock Custom(string declaration, Action<CustomScopeBlock> populate)
        {
            return new CustomScopeBlock(declaration, populate);
        }

        public static CustomScopeBlock Custom(string declaration, Func<IEnumerable<string>> populate)
        {
            return new CustomScopeBlock(declaration, populate);
        }

        public static LoopBlock Loop(string declaration, Action<LoopBlock> populate)
        {
            return new LoopBlock(declaration, populate);
        }

        public static LoopBlock Loop(string declaration, Func<IEnumerable<string>> populate)
        {
            return new LoopBlock(declaration, populate);
        }

        public static IfElseBlock IfElse(string declaration, Action<ScopeBody> populate)
        {
            return new IfElseBlock(declaration, populate);
        }

        public static IfElseBlock IfElse(string declaration, Func<IEnumerable<string>> populate)
        {
            return new IfElseBlock(declaration, populate);
        }

        public static TryCatchBlock TryCatch(Action<ScopeBody> populate)
        {
            return new TryCatchBlock(populate);
        }

        public static TryCatchBlock TryCatch(Func<IEnumerable<string>> populate)
        {
            return new TryCatchBlock(populate);
        }

        public static MethodBlock Method(string declaration, Action<MethodBlock> populate)
        {
            return new MethodBlock(declaration, populate);
        }

        public static MethodBlock Method(string declaration, Func<IEnumerable<string>> populate)
        {
            return new MethodBlock(declaration, populate);
        }

        public static MethodBlock AnnotatedMethod(string annotation, string declaration, Action<MethodBlock> populate)
        {
            return new MethodBlock(declaration, populate, annotation);
        }

        public static MethodBlock AnnotatedMethod(string annotation, string declaration, Func<IEnumerable<string>> populate)
        {
            return new MethodBlock(declaration, populate, annotation);
        }

        public static EnumBlock Enum(string declaration, Action<EnumBlock> populate)
        {
            return new EnumBlock(declaration, populate);
        }

        public static EnumBlock Enum(string declaration, Func<IEnumerable<string>> populate)
        {
            return new EnumBlock(declaration, populate);
        }

        public static EnumBlock AnnotatedEnum(string annotation, string declaration, Action<EnumBlock> populate)
        {
            return new EnumBlock(declaration, populate, annotation);
        }

        public static EnumBlock AnnotatedEnum(string annotation, string declaration, Func<IEnumerable<string>> populate)
        {
            return new EnumBlock(declaration, populate, annotation);
        }

        public static TypeBlock Type(string declaration, Action<TypeBlock> populate)
        {
            return new TypeBlock(declaration, populate);
        }

        public static TypeBlock AnnotatedType(string annotation, string declaration, Action<TypeBlock> populate)
        {
            return new TypeBlock(declaration, populate, annotation);
        }

        public static NamespaceBlock Namespace(string declaration, Action<NamespaceBlock> populate)
        {
            return new NamespaceBlock(declaration, populate);
        }
    }
}
