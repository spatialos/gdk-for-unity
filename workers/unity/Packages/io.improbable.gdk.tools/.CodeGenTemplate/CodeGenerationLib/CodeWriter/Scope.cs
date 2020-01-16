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

        public static CustomScopeBlock Custom(string declaration, Action<CustomScopeBlock> populate)
        {
            return new CustomScopeBlock(declaration, populate);
        }

        public static LoopBlock Loop(string declaration, Action<LoopBlock> populate)
        {
            return new LoopBlock(declaration, populate);
        }

        public static IfElseBlock IfElse(string declaration, Action<ScopeBody> populate)
        {
            return new IfElseBlock(declaration, populate);
        }

        public static TryCatchBlock TryCatch(Action<ScopeBody> populate)
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

        public static EnumBlock Enum(string declaration, Action<EnumBlock> populate)
        {
            return new EnumBlock(declaration, populate);
        }

        public static TypeBlock Type(string declaration, Action<TypeBlock> populate)
        {
            return new TypeBlock(declaration, populate);
        }

        public static NamespaceBlock Namespace(string declaration, Action<NamespaceBlock> populate)
        {
            return new NamespaceBlock(declaration, populate);
        }
    }
}
