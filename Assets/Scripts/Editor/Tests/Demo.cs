using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;

[TestFixture]
public sealed class Demo
{
    [Test]
    void Test()
    {
        var ruleset = new Ruleset();
        var state = new State();

        var rule = ruleset.Query(state);
        rule.Execute();
    }
}
