using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using ConsoleApp1.Core;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ConsoleApp1
{
    /// <summary>
    /// Facilitates the running of sample code for classes within this project.
    /// </summary>
    public static class Runner
    {
        
        public static async Task Run()
        {
            var samples = GetSamples();

            foreach (var sample in samples)
            {
                if (sample.Instance == null)
                {
                    throw new NullReferenceException("Sample must have an instance.");
                }
                
                // Apply method sequences
                var orderedMethods = sample.Methods.OrderBy(method =>
                {
                    var sequence = method.GetCustomAttributes(typeof(SequenceAttribute), false).FirstOrDefault() as SequenceAttribute;
                    return sequence?.Sequence ?? int.MaxValue;
                });
                
                foreach (var method in orderedMethods)
                {
                    try
                    {
                        if (IsAsyncMethod(sample.Instance.GetType(), method))
                        {
                            await (method?.Invoke(sample?.Instance, null) as Task);
                        }
                        else
                        {
                            method.Invoke(sample.Instance, null);
                        }
                    }
                    catch (TargetParameterCountException ex)
                    {
                        
                        Console.WriteLine($"Error: Method {sample.ClassName}.{method.Name} must contain 0 parameters.");
                        // TODO: Propagate the error so that the runner can report on it.
                    }
                    
                }
            }

            await Task.FromResult(new { });
        }
        
        private static bool IsAsyncMethod(Type classType, MemberInfo method)
        {
            var attType = typeof(AsyncStateMachineAttribute);
            
            // Null is returned if the attribute isn't present for the method. 
            var attrib = (AsyncStateMachineAttribute)method.GetCustomAttribute(attType);

            return (attrib != null);
        }

        private static IEnumerable<Sample> GetSamples()
        {
            var types = Assembly.GetCallingAssembly().GetTypes();

            var sampleTypes = types
                .Where(t => t.GetCustomAttributes(typeof(SamplesAttribute), false).Length > 0);

            // Sample classes require empty constructors.
            var sampleInstances = sampleTypes.Select(Activator.CreateInstance);
            var samples = GetSampleRegistrations(sampleInstances);
            return samples;
        }

        private static IEnumerable<Sample> GetSampleRegistrations(IEnumerable<object> sampleInstances)
        {
            foreach (var instance in sampleInstances)
            {
                var publicMethods = instance.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance |
                                                                  BindingFlags.Static | BindingFlags.DeclaredOnly);

                yield return new Sample() { ClassName = instance.GetType().Name, Instance = instance, Methods = publicMethods};
            }
        }
    }
}