using System;
using System.Diagnostics;
using System.IO;


namespace NF64.Windows.cPboWrap
{
    internal sealed class CPboParameter
    {
        public const string PboExtention = ".pbo";


        public string ExePath  { get; }


        public CPboMode Mode { get; }

        public string InputPath { get; }


        public string OutputPath { get; }


        public CPboParameter(string[] args)
        {
            if (args == null)
                throw new ArgumentNullException(nameof(args));
            if (args.Length != 3)
                throw new ArgumentException($"Need 3 Parameters. Ex: [ExePath] [Mode(-e/-p)] [InputPath]");
            this.ExePath = LoadPath(args[0]?.Trim('"'));
            this.Mode = CPboModeUtility.GetMode(args[1]);
            
            var ioPath = LoadIOPath(args[2], this.Mode);
            this.InputPath = ioPath.Item1;
            this.OutputPath = ioPath.Item2;
        }


        public ProcessStartInfo GetProcessStartInfo() 
            => new ProcessStartInfo(this.ExePath, $"{CPboModeUtility.GetCommandString(this.Mode)} \"{this.InputPath}\" \"{this.OutputPath}\"");


        private static string LoadPath(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentException($"'{nameof(path)}' was null or empty.", nameof(path));
            return Path.GetFullPath(path);
        }


        private static Tuple<string, string> LoadIOPath(string path, CPboMode mode)
        {
            var fullPath = LoadPath(path);
            var outputPath = "";
            switch (mode)
            {
            case CPboMode.Extract:
                if (Directory.Exists(fullPath))
                    throw new FileNotFoundException($"'{fullPath}' was not file.", path);
                if (!File.Exists(fullPath))
                    throw new FileNotFoundException($"'{fullPath}' was could be not found.", path);

                var ext = Path.GetExtension(fullPath);
                if (ext.ToLower() != PboExtention)
                    throw new ArgumentException($"input file was not '.pbo'.", nameof(path));
                outputPath = fullPath.Remove(fullPath.Length - ext.Length);
                break;
                
            case CPboMode.Make:
                if (File.Exists(fullPath))
                    throw new FileNotFoundException($"'{fullPath}' was not directory.", path);
                if (!Directory.Exists(fullPath))
                    throw new FileNotFoundException($"'{fullPath}' was could be not found.", path);
                outputPath = fullPath + PboExtention;
                break;

            default:
                throw new ArgumentException($"'{mode}' is undefined.", nameof(mode));
            }

            if (string.IsNullOrEmpty(outputPath))
                throw new InvalidProgramException($"'{nameof(outputPath)}' is null or empty.");
            return new Tuple<string, string>(fullPath, outputPath);
        }


        public override string ToString() => $"ExePath = '{ExePath}', Mode = '{Mode}', InputPath = '{InputPath}', OutputPath = '{OutputPath}'";
    }
}
