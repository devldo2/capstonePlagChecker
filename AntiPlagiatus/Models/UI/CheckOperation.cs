using System;
using System.Collections.Generic;
using System.Threading;

namespace AntiPlagiatus.Models
{
    public class CheckOperation: Base
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public List<IgnoreRule> IgnoreRules { get; set; } = new List<IgnoreRule>();
        public string Key { get; set; }
        public CancellationToken CancellationToken { get; private set; } = new CancellationToken();
    }

    public class IgnoreRule
    {
        public string Id { get; private set; } = Guid.NewGuid().ToString();
        public string Content { get; set; }
        public IgnoreType Type { get; set; }
    }
}
