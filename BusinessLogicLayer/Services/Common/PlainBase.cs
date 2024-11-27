using System;
using System.Collections.Generic;

namespace BusinessLogicLayer.Services.Common
{
    public class PlainBase
    {
        public int Cont { get; set; }
        public enum Pad
        {
            Left,
            Right
        }

        private List<string> _file = new List<string>();
        public List<string> File
        {
            get
            {
                if (_file.Count == 0)
                    _file = GenerateFile();
                return _file;
            }
        }

        public virtual List<string> GenerateFile()
        {
            var file = new List<string> { Header() };
            file.AddRange(Payload());
            file.Add(Footer());
            return file;
        }

        public virtual List<string> Payload()
        {
            return new List<string>();
        }

        public virtual string Header()
        {
            Cont = 1;// siempre inicia en 1
            string registryNumber = GetText(Cont.ToString(), Pad.Left, 7); //F_NUMERO_REG Numero de registro
            string registryType = "0000";                   //F_TIPO_REG Tipo de registro
            string registrySubType = "00";                     //F_SUBTIPO_REG Subtipo de registro
            string registryVersion = "01";                     //F_VERSION_REG Versión del tipo de registro
            string cia = "001";                    //F_CIA Compañía
            Cont++; //aumenta contado para siguientes registros
            return string.Concat(registryNumber, registryType, registrySubType, registryVersion, cia);
        }

        public virtual string Footer()
        {
            string registryNumber = GetText(Cont.ToString(), Pad.Left, 7);   //F_NUMERO_REG Numero de registro
            string registryType = "9999";                                           //F_TIPO_REG Tipo de registro
            string registrySubType = "00";                                             //F_SUBTIPO_REG Subtipo de registro
            string registryVersion = "01";                                             //F_VERSION_REG Versión del tipo de registro
            string cia = "001";                                           //F_CIA Compañía
            return string.Concat(registryNumber, registryType, registrySubType, registryVersion, cia);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="pad">Rigth from string, Left for Number</param>
        /// <param name="max">Max qantity of characters of the field</param>
        /// <returns></returns>
        public static string GetText(string value, Pad pad, int max)
        {
            string result = value;
            try
            {
                if (value != null)
                {
                    if (value.Length > max)
                    {
                        result = value.Substring(0, max);
                    }
                    else if (value.Length < max)
                    {
                        result = value;
                        if (pad == Pad.Left)
                            result = result.PadLeft(max, '0');
                        else
                            result = result.PadRight(max, ' ');
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
                //Logger.Error("Ocurrió un error", ex);
            }

            return result;
        }
    }
}
