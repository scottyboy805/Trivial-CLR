﻿using System;
using System.Reflection;
using TrivialCLR.Runtime;
using UnityEngine;

namespace TrivialCLR.Tests
{
    public class InteropCallTest : PerformanceTest
    {
        // Public
        public int iterations = 1000;
        public bool useDirectCallBinding = false;

        // Properties
        public override string TestName
        {
            get
            {
                return GetType().Name + ((useDirectCallBinding == true) 
                    ? "_DirectCallBinding_Enabled" 
                    : "_DirectCallBinding_Disabled");
            }
        }

        // Methods
        protected override void OnBeforeRunTest()
        {
            if (useDirectCallBinding == true)
            {
                // Add dynamic direct call binding
                InterpretedAppDomain.AddDynamicDirectCallDelegate(
                    typeof(Transform).GetMethod("Translate", new Type[] { typeof(float), typeof(float), typeof(float) }),
                    TransformTranslate_DirectCallBinding);
            }
        }

        protected override void RunTest(MethodInfo method)
        {
            method.Invoke(null, new object[] { transform, iterations });
        }

        private static void TransformTranslate_DirectCallBinding(StackData[] stack, int offset)
        {
            ((Transform)stack[offset].refValue).Translate(
                stack[offset + 1].value.Single,
                stack[offset + 2].value.Single,
                stack[offset + 3].value.Single);
        }
    }
}
