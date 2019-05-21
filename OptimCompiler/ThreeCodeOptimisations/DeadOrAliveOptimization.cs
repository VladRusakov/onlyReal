﻿using SimpleLang.Visitors;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleLang.ThreeCodeOptimisations
{
    class DeadOrAliveOptimization
    {
        /// <summary>
        /// Возвращает результат каскадного удаления живых и мертвых переменных,
        /// выполненного над передаваемым блоком <paramref name="block"/>.
        /// </summary>
        /// <param name="block"></param>
        public static LinkedList<ThreeCode> DeleteDeadVariables(LinkedList<ThreeCode> block)
        {
            var variables = new Dictionary<string, bool>();
            // сначала добавляются все аргументы, которые хоть раз были определены
            // это консервативное предположение о последующем использовании этих определений
            foreach(var line in block)
                if(!line.result.StartsWith("temp_"))
                    variables[line.result] = true;
            return DeleteDeadVariables(block, variables);
        }

        /// <summary>
        /// Возвращает результат каскадного удаления живых и мертвых переменных,
        /// выполненного над передаваемым блоком <paramref name="block"/>.
        /// При этом используется предварительная информация о "живости" переменных 
        /// из словаря <paramref name="variables"/>
        /// </summary>
        /// <param name="block"></param>
        /// <param name="variables"></param>
        public static LinkedList<ThreeCode> DeleteDeadVariables(LinkedList<ThreeCode> block, Dictionary<string, bool> variables)
        {
            var result = new LinkedList<ThreeCode>(block);
            LinkedListNode<ThreeCode> current = result.Last;
            
            while (current != null)
            {
                var previous = current.Previous;
                if (!(variables.ContainsKey(current.Value.result) && variables[current.Value.result]) && current.Value.result != "")
                    if (current.Value.label != null && current.Value.label != "")
                        current.Value = new ThreeCode(current.Value.label, "", ThreeOperator.None, null, null);
                    else
                        result.Remove(current);
                else
                    AddThreeCodeLine(current.Value, variables);
                current = previous;
            }
            return result;
        }

        /// <summary>
        /// Добавляет информацию о переменных строки кода <paramref name="line"/> в
        /// словарь <paramref name="variables"/>
        /// </summary>
        /// <param name="line"></param>
        /// <param name="variables"></param>
        private static void AddThreeCodeLine(ThreeCode line, Dictionary<string, bool> variables)
        {
            variables[line.result] = false;

            if (line.arg1 != null && line.arg1 is ThreeAddressStringValue && line.arg1.ToString() != "")
                variables[line.arg1.ToString()] = true;

            if (line.arg2 != null && line.arg2 is ThreeAddressStringValue && line.arg2.ToString() != "")
                variables[line.arg2.ToString()] = true;
        }
    }
}
