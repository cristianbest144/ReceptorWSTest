using BusinessLogicLayer.Models;
using DataAccessLayer.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services.Common
{
    public class DynamicPlainFile : PlainBase
    {
        #region properties
        public PlainDataInput Input { get; }
        public PlainSchema Schema { get; }
        public int LineCounter { get; set; }

        private readonly PlainSectionSchema _headerSection;
        private readonly PlainSectionSchema _footerSection;
        #endregion

        #region constructor
        public DynamicPlainFile()
        {
        }
        public DynamicPlainFile(PlainDataInput input, PlainSchema schema)
        {
            Input = input;
            Schema = schema;
            this.Cont = 1;
            LineCounter = 1;
            _headerSection = Schema.Sections.OrderBy(x => x.Order).FirstOrDefault();
            _footerSection = Schema.Sections.OrderByDescending(x => x.Order).FirstOrDefault();
        }

        #endregion

        public override string Header()
        {
            var headerData = Input.Sections.FirstOrDefault(x => x.KeyName == _headerSection.KeyName);
            StringBuilder sb = new StringBuilder();
            ProcessLine(_headerSection, headerData, sb);
            this.Cont++;
            return sb.ToString();
        }

        public override string Footer()
        {
            var footerData = Input.Sections.FirstOrDefault(x => x.KeyName == _footerSection.KeyName);
            StringBuilder sb = new StringBuilder();
            ProcessLine(_footerSection, footerData, sb);
            this.Cont++;
            return sb.ToString();
        }

        public override List<string> Payload()
        {
            var result = new List<string>();
            var lst = new List<int> { _headerSection.Id, _footerSection.Id };
            var payloadSections = Schema.Sections
                                .OrderBy(x => x.Order)
                                .Where(x => !lst.Contains(x.Id))
                                .ToList();

            foreach (var section in payloadSections)
            {
                //un archivo puede tener repetida variables veces la misma sección
                var payloadData = Input.Sections
                                      .Where(x => x.KeyName == section.KeyName)
                                      .ToList();
                foreach (var sectionData in payloadData)
                {
                    StringBuilder sb = new StringBuilder();
                    ProcessLine(section, sectionData, sb);
                    result.Add(sb.ToString());
                    this.Cont++;
                    this.LineCounter++;//PILAS: si hay deferentes secciones este contador no se debe utilizar deberian enviarlo en la data del archivo
                }
            }
            return result;
        }

        private void ProcessLine(PlainSectionSchema sectionSchema, SectionData sectionData, StringBuilder sb)
        {
            foreach (var field in sectionSchema.Fields.OrderBy(x => x.Value.Order))
            {
                var _key = field.Key.Trim();
                try
                {
                    var value = sectionData.FieldsDictionary[field.Key.Trim()];
                    var pad = Pad.Right;
                    switch (field.Value.Type)
                    {
                        case PlainFieldType.GlobalCounter:
                            {
                                pad = Pad.Left;
                                value = $"{this.Cont}";
                            }
                            break;
                        case PlainFieldType.LineCounter:
                            {
                                pad = Pad.Left;
                                value = $"{this.LineCounter}";
                            }
                            break;
                        case PlainFieldType.Int:
                        case PlainFieldType.Decimal:
                            {
                                pad = Pad.Left;
                                value = FormatValue(value, field.Value.Format);
                            }
                            break;
                    }
                    value = GetText(value, pad, field.Value.Size);
                    sb.Append(value);
                }
                catch (Exception ex)
                {
                    throw new Exception($"{_key} {ex.ToString()}");
                }
            }
        }

        private static string FormatValue(string value, string format)
        {
            if (String.IsNullOrEmpty(format))
                return value;

            var v = value.Replace(",", ".");
            var val = v.Split('.');
            var (decimalCount, valueCount) = GetFormatVal(format);

            var valx = "";
            if (val.Length > 1)
                valx = val[0].PadLeft(valueCount, '0') + '.' + val[1].PadRight(decimalCount, '0');
            else
                valx = val[0].PadLeft(valueCount, '0') + '.' + "".PadRight(decimalCount, '0');

            if (format.Contains('+'))
                valx = $"+{valx}";

            return valx;
        }

        private static (int decilmalCount, int valueCount) GetFormatVal(string format)
        {
            var decimalCount = format.Contains(".") ? format.Split('.')[1].Length : 0;
            return (decimalCount,
                    format.Length - (1 + decimalCount));
        }
    }
}
