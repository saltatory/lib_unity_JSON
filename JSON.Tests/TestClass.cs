using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Mobilityware.JSON.Tests
{

    public class TestColor
    {
        public float Red = 0.0f;
        public float Green = 0.0f;
        public float Blue = 0.0f;
        public float Alpha = 0.0f;
    }

    public class TestPayLine
    {
        public string Positions = "";
        public string Offsets = "";
        public TestColor Color = new TestColor();
    }

    public class TestPayLine2
    {
        public string Positions;
        public Vector3 Offsets = Vector3.one;
        public TestColor Color = new TestColor();
    }

    [TestFixture]
    public class TestClass
    {
        IJSON json = new JSON();
        System.Reflection.Assembly assembly;
        System.IO.StreamReader textStreamReader;

        public TestClass()
        {
            assembly = System.Reflection.Assembly.GetExecutingAssembly();
        }


        [Test]
        [MaxTime(1000)]
        public void TestParseLPVar()
        {
            string name = "JSON.Tests.Resources.JSONLPVarTest.json";

            using(Stream stream = assembly.GetManifestResourceStream(name))
            using (textStreamReader = new System.IO.StreamReader(stream))
            {
                string text = textStreamReader.ReadToEnd();

                Dictionary<string, object> data = (Dictionary<string, object>)json.DeserializeFromJson(text);
                Dictionary<string, object> schema = (Dictionary<string, object>)data["_schema"];
                Dictionary<string, object> color = (Dictionary<string, object>)schema["Color"];
                List<object> list = (List<object>)color["_list"];
                if ((string)(list[2]) == "Blue")
                {
                    Assert.Pass();
                }
                else {
                    Assert.Fail();
                }
            }
        }

        [Test]
        [MaxTime(1000)]
        public void TestParseSlotsGameInfoDefault()
        {
            string name = "JSON.Tests.Resources.JSONSlotsGameDefaultsTest.json";

            using (Stream stream = assembly.GetManifestResourceStream(name))
            using (textStreamReader = new System.IO.StreamReader(stream))
            {
                {
                    string text = textStreamReader.ReadToEnd();
                    Dictionary<string, object> data = (Dictionary<string, object>)json.DeserializeFromJson(text);
                    Dictionary<string, object> levels = (Dictionary<string, object>)data["ExperiencePerLevel"];
                    int score = (int)(levels["13"]);
                    if (score == 70000)
                    {
                        Assert.Pass();
                    }
                    else {
                        Assert.Fail();
                    }
                }
            }
        }

        [Test]
        [MaxTime(1000)]
        public void TestParseSlotsPayline()
        {
            string name = "JSON.Tests.Resources.JSONSlotsPayLineTest.json";

            using (Stream stream = assembly.GetManifestResourceStream(name))
            using (textStreamReader = new System.IO.StreamReader(stream))
            {
                string text = textStreamReader.ReadToEnd();
                Dictionary<string, object> data = (Dictionary<string, object>)json.DeserializeFromJson(text);
                Dictionary<string, object> paylines = (Dictionary<string, object>)data["Paylines"];

                string line4 = json.SerializeToJson(paylines["Line 4"]);

                TestPayLine payline = json.DeserializeFromJson<TestPayLine>(line4);

                if (payline.Color.Red == 0.184f)
                {
                    Assert.Pass();
                }
                else {
                    Assert.Fail();
                }
            }
        }

        [Test]
        [MaxTime(1000)]
        public void TestParseSlotsPayline2()
        {
            TestPayLine2 tp = new TestPayLine2();
            tp.Positions = null;
            tp.Color.Blue = 111.1f;
            string ss = json.SerializeToJson(tp);
            TestPayLine2 tp2 = json.DeserializeFromJson<TestPayLine2>(ss);

            if (tp2.Color.Blue == tp.Color.Blue && tp2.Positions == null)
            {
                Assert.Pass();
            }
            else {
                Assert.Fail();
            }
        }

        [Test]
        [MaxTime(1000)]
        public void TestParseNestedJSON()
        {
            TestPayLine2 parent = new TestPayLine2();
            TestPayLine2 child = new TestPayLine2();
            child.Color.Blue = 111.1f;
            parent.Positions = json.SerializeToJson(child);
            parent.Color.Red = 222.1f;
            string ss = json.SerializeToJson(parent);
            TestPayLine2 parent2 = json.DeserializeFromJson<TestPayLine2>(ss);
            TestPayLine2 child2 = json.DeserializeFromJson<TestPayLine2>(parent2.Positions);

            if (child2.Color.Blue == child.Color.Blue && parent2.Color.Red == parent.Color.Red)
            {
                Assert.Pass();
            }
            else {
                Assert.Fail();
            }
        }


    }
}
