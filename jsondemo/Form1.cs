using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace jsondemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }



       

        private void button1_Click(object sender, EventArgs e)
        {
            string xmlText = textBox1.Text;

            string jsonText = xmlText.XmlToJson();

            textBox2.Text = jsonText;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string xml = @"<names>
                            <name>
                                foo
                            </name>
                            <name>
                                bar
                            </name>
                        </names>";

            textBox1.Text = xml;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string jsonText = textBox2.Text;

            string xmlText = jsonText.JsonToXml();

            textBox1.Text = xmlText;
        }




        DynamicMessage original = new DynamicMessage
        {
            Id = Guid.NewGuid(),
            Priority = 3,
            ReplyTo = "http://path/to/reply.svc",
            RetryAttempts = 1,
            Type = typeof(MessageBody),
            Body = new MessageBody
            {
                Action = "Alphabet",
                Arguments = { "a", "b", "c" }
            }
        };

        IList<DynamicMessage> collection = new List<DynamicMessage>(new []{
            
            new DynamicMessage
            {
                Id = Guid.NewGuid(),
                Priority = 3,
                ReplyTo = "http://path/to/reply.svc",
                RetryAttempts = 1,
                Type = typeof(MessageBody),
                Body = new MessageBody
                {
                    Action = "Alphabet",
                    Arguments = { "a", "b", "c" }
                }
            },
            new DynamicMessage
            {
                Id = Guid.NewGuid(),
                Priority = 1,
                ReplyTo = "http://blah/to/reply.svc",
                RetryAttempts = 20,
                Type = typeof(MessageBody),
                Body = new MessageBody
                {
                    Action = "Monkey",
                    Arguments = { "a", "b", "c" }
                }
            }
    
        });

        private void button4_Click(object sender, EventArgs e)
        {

            var jsv = TypeSerializer.SerializeToString(original);
            var dynamicType = TypeSerializer.DeserializeFromString<DynamicMessage>(jsv);
            var genericType = TypeSerializer.DeserializeFromString<GenericMessage<MessageBody>>(jsv);
            var strictType = TypeSerializer.DeserializeFromString<StrictMessage>(jsv);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var csv = CsvSerializer.SerializeToCsv(collection);
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(csv ?? ""));
            var coll = CsvSerializer.DeserializeFromStream(typeof(DynamicMessage), stream);

        }
    }
}
