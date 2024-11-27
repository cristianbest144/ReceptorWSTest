using BusinessLogicLayer.Dto;
using BusinessLogicLayer.Models;
using BusinessLogicLayer.Services.Common;
using DataAccessLayer.Common;
using DataAccessLayer.Entites;
using DataAccessLayer.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogicLayer.Services
{
    public class PlainBuilderManagerServices
    {
        public readonly PlainRepository _plainRepository;
        public readonly SectionRepository _sectionRepository;
        public PlainBuilderManagerServices()
        {
            _plainRepository = new PlainRepository();
            _sectionRepository = new SectionRepository();
        }
        public PlainSchema Builder(string keyName)
        {
            var plain = _plainRepository.GetKey(keyName);
            if (plain == null)
                throw new Exception("El plano no existe");

            return new PlainSchema(plain, plain.Sections.ToList());
        }
        public DynamicPlainFile Execute(PlainDataInput input)
        {
            var schema = Builder(input.KeyName);
            ValidatePlain(input, schema);
            return new DynamicPlainFile(input, schema);
        }
        public List<PlainSectionDetailDto> GetRequiredFieldsAsync(string keyname)
        {
            var plain = _plainRepository.GetKey(keyname);
            if (plain == null)
                throw new Exception("El plano no existe");

            var section = plain
                            .Sections
                            .Where(x => x.Fields.Any(y => y.IsVariable &&
                                                                      y.Type != PlainFieldType.GlobalCounter &&
                                                                      y.Type != PlainFieldType.LineCounter))
                            .Select(t => new PlainSectionDetailDto
                            {
                                AllowMultipleRows = t.AllowMultipleRows,
                                keyName = t.KeyName.Trim(),
                                Name = t.Name,
                                Order = t.Order,
                                PlainId = t.PlainId,
                                Fields = t.Fields.Select(m => new PlainFieldsDetailDto
                                {
                                    Description = m.Description,
                                    DefaultValue = m.DefaultValue,
                                    FieldName = m.FieldName.Trim(),
                                    Order = m.Order,
                                    Format = m.Format,
                                    IsVariable = m.IsVariable,
                                    Observations = m.Observations,
                                    PlainSectionId = m.PlainSectionId,
                                    Size = m.Size,
                                    Type = m.Type,
                                }).ToList()
                            })
                            .ToList();
            return section.ToList();
        }
        private void ValidatePlain(PlainDataInput input, PlainSchema schema)
        {
            //TODO: test as parallel
            foreach (var section in schema.Sections)
            //foreach (var section in schema.Sections.AsParallel())
            {
                if (section.Fields.Any(x => x.Value.IsVariable) && input.Sections.FirstOrDefault(x => x.KeyName.Trim() == section.KeyName.Trim()) is null)
                {
                    if (!section.IsRequired) continue;

                    throw new Exception($"La seccion {section.Id} le falta un campo requerido");
                }

                if (input.Sections.FirstOrDefault(x => x.KeyName.Trim() == section.KeyName.Trim()) is null)
                {
                    input.Sections.Add(new SectionData { KeyName = section.KeyName, Fields = section.Fields.Select(x => new FieldData { KeyName = x.Key, Value = x.Value.DefaultValue }).ToList() });
                }
                else
                {
                    //Validar si la seccion tiene algun campo que no sea una variable o contador
                    //file can contain the same section more than one time
                    foreach (var sectionInFile in input.Sections.Where(x => x.KeyName == section.KeyName))
                    //foreach (var sectionInFile in input.Sections.Where(x => x.KeyName == section.KeyName).AsParallel())
                    {
                        foreach (var field in section.Fields)
                        {
                            var _field = sectionInFile.Fields.FirstOrDefault(x => x.KeyName.Trim() == field.Key.Trim());
                            if (_field == null && field.Value.IsVariable)
                            {
                                throw new Exception($"Falta el campo {field.Key} en la seccion {section.Id} el cual es requerido");
                            }

                            //if (_field != null && field.Value.IsVariable && _field.Value is null)
                            //{
                            //    throw new Exception($"Falta el valor del campo {field.Key} en la seccion{section.Id} el cual es requerido");
                            //}

                            if (_field != null && field.Value.IsVariable && _field.Value is null)
                            {
                                _field.Value = field.Value.DefaultValue;
                            }

                            if (_field != null && !field.Value.IsVariable)
                            {
                                _field.Value = field.Value.DefaultValue;
                            }

                            if (_field is null)
                            {
                                sectionInFile.Fields.Add(new FieldData { KeyName = field.Key, Value = field.Value.DefaultValue });
                            }
                        }
                    }
                }
            }
        }
    }

    public class PlainSchema
    {
        public List<PlainSectionSchema> Sections { get; set; }
        public int Plain { get; }

        public PlainSchema(Plain plain, List<PlainSection> sections)
        {
            this.Plain = plain.Id;
            Sections = new List<PlainSectionSchema>();

            Sections = sections.OrderBy(x => x.Order)
                               .Select(x => new PlainSectionSchema(x.Id, x.Order, x.Name, x.KeyName.Trim(), x.Fields, x.IsRequired))
                               .ToList();
        }
    }
}
