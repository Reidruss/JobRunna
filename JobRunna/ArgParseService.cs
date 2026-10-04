using System;
using System.Collections.Generic;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.RepresentationModel;
using JobRunna.Models;

namespace JobRunna;

public static class ArgParseService
{
    public static async Task<SubmissionModel> Parse(string arg)
    {
        if (Path.GetExtension(arg) != ".yaml") throw new ArgumentException($"Invalid file extension {arg}");
        
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .Build();
        
        return deserializer
            .Deserialize<SubmissionModel>(new StringReader(await File.ReadAllTextAsync(arg)));
        
    }
}