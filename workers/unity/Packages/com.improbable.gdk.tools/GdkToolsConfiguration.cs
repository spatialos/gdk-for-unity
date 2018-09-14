﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Improbable.Gdk.Tools
{
    public class GdkToolsConfiguration
    {
        public string SchemaStdLibDir;
        public List<string> SchemaSourceDirs = new List<string>();
        public string CodegenOutputDir;

        private static string JsonFilePath = Path.GetFullPath("Assets/Config/GdkToolsConfiguration.json");

        private const string SchemaStdLibDirJsonField = "schema_standard_lib_directory";
        private const string SchemaSourceDirsJsonField = "schema_source_directories";
        private const string CodegenOutputDirJsonField = "codegen_output_directory";

        public GdkToolsConfiguration()
        {
            ResetToDefault();
        }

        public void Save()
        {
            var json = ToJson();
            if (File.Exists(JsonFilePath))
            {
                File.Delete(JsonFilePath);
            }

            File.WriteAllText(JsonFilePath, MiniJSON.Json.Serialize(json));
        }

        internal List<string> Validate()
        {
            var errors = new List<string>();
            if (string.IsNullOrEmpty(SchemaStdLibDir))
            {
                errors.Add($"{GdkToolsConfigurationWindow.SchemaStdLibDirLabel} cannot be empty.");
            }

            if (string.IsNullOrEmpty(CodegenOutputDir))
            {
                errors.Add($"{GdkToolsConfigurationWindow.CodegenOutputDirLabel} cannot be empty.");
            }

            if (SchemaSourceDirs.Any(string.IsNullOrEmpty))
            {
                errors.Add($"Cannot have any empty entry in {GdkToolsConfigurationWindow.SchemaSourceDirsLabel}.");
            }

            if (SchemaSourceDirs.Count == 0)
            {
                errors.Add($"You must have at least one item in {GdkToolsConfigurationWindow.SchemaSourceDirsLabel}.");
            }

            return errors;
        }

        internal void ResetToDefault()
        {
            SchemaStdLibDir = DefaultValues.SchemaStdLibDir;
            CodegenOutputDir = DefaultValues.CodegenOutputDir;
            SchemaSourceDirs.Clear();
            SchemaSourceDirs.Add(DefaultValues.SchemaSourceDir);
        }

        private void LoadFromJson(Dictionary<string, object> data)
        {
            if (!TryGetFieldFromJson<string>(data, SchemaStdLibDirJsonField, out var schemaStdLibDir))
            {
                ThrowBadJsonException(SchemaStdLibDirJsonField);
            }

            if (!TryGetFieldFromJson<IList>(data, SchemaSourceDirsJsonField, out var schemaSourceDirsIList))
            {
                ThrowBadJsonException(SchemaSourceDirsJsonField);
            }

            if (!TryGetFieldFromJson<string>(data, CodegenOutputDirJsonField, out var codegenOutputDir))
            {
                ThrowBadJsonException(CodegenOutputDirJsonField);
            }

            // Can't cast from object to List<string>. Need to go to intermediate IList and iterate over
            // that.
            var actualSchemaSourceDirs = schemaSourceDirsIList.Cast<string>().ToList();

            SchemaStdLibDir = schemaStdLibDir;
            SchemaSourceDirs = actualSchemaSourceDirs;
            CodegenOutputDir = codegenOutputDir;
        }

        private void ThrowBadJsonException(string jsonField)
        {
            throw new JsonSerializationException($"Could not find field {jsonField} in JSON loaded at {JsonFilePath}");
        }

        private bool TryGetFieldFromJson<TValue>(Dictionary<string, object> jsonData, string jsonFieldLabel,
            out TValue value)
        {
            if (!jsonData.TryGetValue(jsonFieldLabel, out var obj))
            {
                value = default(TValue);
                return false;
            }

            if (obj is TValue castedValue)
            {
                value = castedValue;
                return true;
            }

            value = default(TValue);
            return false;
        }

        private Dictionary<string, object> ToJson()
        {
            var data = new Dictionary<string, object>();
            data[SchemaStdLibDirJsonField] = SchemaStdLibDir;
            data[SchemaSourceDirsJsonField] = SchemaSourceDirs;
            data[CodegenOutputDirJsonField] = CodegenOutputDir;

            return data;
        }

        public static GdkToolsConfiguration GetOrCreateInstance()
        {
            return File.Exists(JsonFilePath) ? LoadFile() : CreateInstance();
        }

        private static GdkToolsConfiguration LoadFile()
        {
            var data = MiniJSON.Json.Deserialize(File.ReadAllText(JsonFilePath));
            var config = new GdkToolsConfiguration();
            config.LoadFromJson(data);

            return config;
        }

        private static GdkToolsConfiguration CreateInstance()
        {
            var config = new GdkToolsConfiguration();
            var jsonString = MiniJSON.Json.Serialize(config.ToJson());

            File.WriteAllText(JsonFilePath, jsonString);

            return config;
        }

        private static class DefaultValues
        {
            public const string SchemaStdLibDir = "../../build/dependencies/schema/standard_library";
            public const string CodegenOutputDir = "Assets/Generated/Source";
            public const string SchemaSourceDir = "../../schema";
        }

        private class JsonSerializationException : Exception
        {
            public JsonSerializationException(string message) : base(message)
            {
            }
        }
    }
}
