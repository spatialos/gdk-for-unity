using System;

namespace Improbable.Gdk.CodeGenerator
{
    /// <summary>
    ///     Tools for generating Unity meta files.
    /// </summary>
    internal static class MetaFileTools
    {
        /// <summary>
        ///     Generates a Guid for a file generated for a component.
        /// </summary>
        public static Guid GenerateGuidForComponent(int componentId, string fileType)
        {
            return StableGuidGenerator.GenerateGuid(StableGuidGenerator.UrlNamespace,
                string.Concat(componentId, fileType));
        }

        /// <summary>
        ///     Generates a Guid for a file generated for a schema type.
        /// </summary>
        public static Guid GenerateGuidForType(string qualifiedName, string fileType)
        {
            return StableGuidGenerator.GenerateGuid(StableGuidGenerator.UrlNamespace,
                string.Concat(qualifiedName, fileType, "Type"));
        }

        /// <summary>
        ///     Generates a Guid for a directory generated for a schema package.
        /// </summary>
        public static Guid GenerateGuidForPackage(string directoryName, string fileType)
        {
            return StableGuidGenerator.GenerateGuid(StableGuidGenerator.UrlNamespace,
                string.Concat(directoryName, fileType, "Package"));
        }

        /// <summary>
        ///     Generates the contents of the Unity meta file.
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static string GetMetaFileContentsForComponent(int componentId, string fileType)
        {
            return GenerateMetaFileContents(GenerateGuidForComponent(componentId, fileType));
        }

        /// <summary>
        ///     Generates the contents of the Unity meta file.
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static string GetMetaFileContentsForType(string qualifiedName, string fileType)
        {
            return GenerateMetaFileContents(GenerateGuidForType(qualifiedName, fileType));
        }

        /// <summary>
        ///     Generates the contents of the Unity meta file.
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static string GetMetaFileContentsForPackage(string packageName, string fileType)
        {
            return GenerateMetaFileContents(GenerateGuidForPackage(packageName, fileType));
        }

        /// <summary>
        ///     Generates the contents of the Unity meta file.
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        private static string GenerateMetaFileContents(Guid guid)
        {
            return string.Format("guid: {0:N}", guid); // e.g. guid: 803be2087a8345658f4488a917b2531c
        }
    }
}
