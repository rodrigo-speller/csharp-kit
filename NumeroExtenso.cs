/*
Copyright (c) 2016 Rodrigo Speller

 A permissão é concedida, a título gratuito, para qualquer pessoa que obtenha
 uma cópia deste software e arquivos de documentação associados (o
 "Software"), para lidar com o Software sem restrição, incluindo, sem
 limitação dos direitos de uso, copiar, modificar, mesclar, publicar,
 distribuir, sublicenciar e/ou vender cópias do Software, e para permitir que
 as pessoas às quais o Software é fornecido a fazê-lo, mediante as seguintes
 condições:

 O aviso de direito autoral acima e este aviso de permissão devem ser
 incluídos em todas as cópias ou partes substanciais do Software.

 O SOFTWARE É FORNECIDO "COMO ESTÁ", SEM QUALQUER TIPO DE GARANTIA, EXPRESSA
 OU IMPLÍCITA, INCLUINDO MAS NÃO LIMITADA A GARANTIAS COMERCIAIS, ADEQUAÇÃO A
 UMA FINALIDADE ESPECÍFICA E NÃO VIOLAÇÃO. EM HIPÓTESE NENHUMA, OS AUTORES OU
 DETENTORES DOS DIREITOS AUTORAIS SE RESPONSABILIZAM POR QUALQUER
 REIVINDICAÇÃO, DANOS OU OUTRAS OBRIGAÇÕES, SEJA EM UM ATO CONTRATUAL, ILÍCITO
 OU OUTRA FORMA, DECORRENTE DE, OU ASSOCIADO COM O SOFTWARE OU O USO DE OUTROS
 PROCEDIMENTOS DO SOFTWARE.
*/
using System;
using System.Collections.Generic;
using System.IO;

namespace Extensions
{
    public static class NumeroExtenso
    {
        private const string N0 = "zero";
        private const string N1 = "um";
        private const string N2 = "dois";
        private const string N3 = "tres";
        private const string N4 = "quatro";
        private const string N5 = "cinco";
        private const string N6 = "seis";
        private const string N7 = "sete";
        private const string N8 = "oito";
        private const string N9 = "nove";
        private const string N10 = "dez";
        private const string N11 = "onze";
        private const string N12 = "doze";
        private const string N13 = "treze";
        private const string N14 = "catorze";
        private const string N15 = "quinze";
        private const string N16 = "dezesseis";
        private const string N17 = "dezessete";
        private const string N18 = "dezoito";
        private const string N19 = "dezenove";
        private const string N20 = "vinte";
        private const string N30 = "trinta";
        private const string N40 = "quarenta";
        private const string N50 = "cinquenta";
        private const string N60 = "sessenta";
        private const string N70 = "setenta";
        private const string N80 = "oitenta";
        private const string N90 = "noventa";
        private static string[] N100 = { "cem", "cento" };
        private const string N200 = "duzentos";
        private const string N300 = "trezentos";
        private const string N400 = "quatrocentos";
        private const string N500 = "quinhentos";
        private const string N600 = "seiscentos";
        private const string N700 = "setecentos";
        private const string N800 = "oitocentos";
        private const string N900 = "novecentos";

        private static string[] C3 = { "mil", "mil" };
        private static string[] C6 = { "milhão", "milhões" };
        private static string[] C9 = { "bilhão", "bilhões" };
        private static string[] C12 = { "trilhão", "trilhões" };
        private static string[] C15 = { "quatrilhão", "quatrilhões" };
        private static string[] C18 = { "quintilhão", "quintilhões" };
        private static string[] C21 = { "sextilhão", "sextilhões" };
        private static string[] C24 = { "septilhão", "septilhões" };

        private static string[] LITERAIS = { N0, N1, N2, N3, N4,
                                             N5, N6, N7, N8, N9,
                                             N10, N11, N12, N13, N14,
                                             N15, N16, N17, N18, N19 };

        private static string[] DEZENAS = { null, null, N20, N30, N40,
                                            N50, N60, N70, N80, N90 };

        private static string[] CENTENAS = { null, N100[1], N200, N300, N400,
                                             N500, N600, N700, N800, N900 };

        private static string[][] ESCALAS = { null, C3, C6, C9, C12, C15, 
                                              C18, C21, C24 };

        private const string NADA = "nada";

        public static string Extenso(this ulong numero)
        {

            if (numero == 0) // zero
                return LITERAIS[0];

            using (StringWriter writer = new StringWriter())
            {
                var escalas = new Stack<KeyValuePair<ushort, List<string>>>();

                // INTERPRETAÇÃO DAS ESCALAS

                while (numero > 0)
                {
                    var classes = new List<string>(3);
                    ushort parte = (ushort)(numero % 1000);
                    numero /= 1000;

                    if (parte == 0)
                    {
                        // nada
                    }
                    else if (parte == 100)
                    {
                        classes.Add(N100[0]);
                    }
                    else if (parte < 20)
                    {
                        classes.Add(LITERAIS[parte]);
                    }
                    else
                    {
                        // centenas
                        byte n = (byte)(parte / 100);
                        if (n > 0)
                            classes.Add(CENTENAS[n]);

                        n = (byte)(parte % 100);

                        // dezenas e unidades
                        if (n > 0)
                        {
                            if (n < 20)
                            {
                                // 1 a 19
                                classes.Add(LITERAIS[n]);
                            }
                            else
                            {
                                // dezenas
                                n = (byte)(parte % 100 / 10);
                                if (n > 0)
                                    classes.Add(DEZENAS[n]);

                                // unidades
                                n = (byte)(parte % 10);
                                if (n > 0)
                                    classes.Add(LITERAIS[n]);
                            }
                        }

                    }

                    escalas.Push(
                        new KeyValuePair<ushort, List<string>>(parte, classes)
                    );
                }

                // PREPARAÇÃO DO RETORNO

                var arrEscalas = escalas.ToArray();

                int countEscalas = arrEscalas.Length;
                var countEscalasValidas = 0;

                int i = 0;

                // conta a quantidade de escalas maior que zero
                for (i = 0; i < countEscalas; i++)
                {
                    if (arrEscalas[i].Key > 0)
                        countEscalasValidas++;
                }

                // retorno

                for (i = 0; i < countEscalas; i++)
                {
                    var escala = arrEscalas[i];

                    // ignora escalas com valor zero
                    if (escala.Key == 0)
                        continue;

                    // índice da escala
                    int iEscala = countEscalas - i - 1;

                    // separadores entre as escalas
                    if (i > 0 && escala.Key > 0)
                        // Valor de centena absoluta ou menor que cem: 
                        // utiliza-se a conjunção "e" para ligar uma classe e
                        // outra (unidade simples, milhar, milhão etc.) quando
                        // o valor da classe anterior (à direita) for uma
                        // centena absoluta ou menor que cem.
                        // A aplica-se apenas para as duas escalas menores
                        // (unidades e milhar) e para as duas escalas menores
                        // válidas.
                        if (
                            (iEscala < 2 || countEscalasValidas < 2)
                            && (
                                escala.Key < 100
                                || escala.Key / 100 * 100 == escala.Key
                            )
                        )
                            writer.Write(" e ");
                        else
                            writer.Write(", ");

                    if (iEscala == 1 && escala.Key == 1)
                        // mil
                        writer.Write(ESCALAS[iEscala][0]);
                    else
                    {
                        // valor da escala
                        writer.Write(string.Join(" e ", escala.Value));

                        // escala (mil, milhão, bilhão...)
                        if (iEscala != 0)
                        {
                            writer.Write(' ');
                            writer.Write(
                                ESCALAS[iEscala][escala.Key == 1 ? 0 : 1]
                            );
                        }
                    }

                    countEscalasValidas--;
                }

                // retorno
                return writer.ToString();
            }
        }

        public static string Extenso(this long valor) { 
            return Extenso(Convert.ToUInt64(valor));
        }

        public static string Extenso(this uint valor)
        {
            return Extenso(Convert.ToUInt64(valor));
        }

        public static string Extenso(this int valor)
        {
            return Extenso(Convert.ToUInt64(valor));
        }

        public static string Extenso(this ushort valor)
        {
            return Extenso(Convert.ToUInt64(valor));
        }

        public static string Extenso(this short valor)
        {
            return Extenso(Convert.ToUInt64(valor));
        }

        public static string Extenso(this byte valor)
        {
            return Extenso(Convert.ToUInt64(valor));
        }

        public static string Extenso(this sbyte valor)
        {
            return Extenso(Convert.ToUInt64(valor));
        }

        public static string MoedaExtenso(this decimal valor)
        {
            ulong inteiro = decimal.ToUInt64(valor);
            byte fracao = (byte)((valor - decimal.Truncate(valor)) * 100);

            if (inteiro == 0 && fracao == 0)
                return NADA;

            using (StringWriter writer = new StringWriter())
            {
                if (inteiro > 0)
                {
                    writer.Write(Extensions.NumeroExtenso.Extenso(inteiro));
                    if (inteiro >= 1000000 & inteiro % 1000000 == 0)
                        writer.Write(" de reais");
                    else
                        writer.Write(inteiro == 1 ? " real" : " reais");
                }

                if (fracao > 0)
                {
                    if (inteiro > 0)
                        writer.Write(" e ");

                    writer.Write(Extensions.NumeroExtenso.Extenso(fracao));
                    writer.Write(fracao == 1 ? " centavo" : " centavos");
                }

                return writer.ToString();
            }
        }

        public static string MoedaExtenso(this double valor)
        {
            return MoedaExtenso((decimal)valor);
        }

        public static string MoedaExtenso(this float valor)
        {
            return MoedaExtenso((decimal)valor);
        }
    }
}
