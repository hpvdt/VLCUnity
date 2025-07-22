using System;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using LibVLCSharp;

namespace Videolabs.VLCUnity.Tests
{
    public class OnLoadTests
    {
        [Test]
        public void UnityColorSpaceEnum_HasCorrectValues()
        {
            // Assert enum values
            Assert.AreEqual(0, (int)OnLoad.UnityColorSpace.Gamma, "Gamma should have value 0");
            Assert.AreEqual(1, (int)OnLoad.UnityColorSpace.Linear, "Linear should have value 1");
        }

        [Test]
        public void SetColorSpace_WithGamma_DoesNotThrow()
        {
            // Act & Assert
            Assert.DoesNotThrow(() => OnLoad.SetColorSpace(OnLoad.UnityColorSpace.Gamma),
                "SetColorSpace should not throw when called with Gamma value");
        }

        [Test]
        public void SetColorSpace_WithLinear_DoesNotThrow()
        {
            // Act & Assert
            Assert.DoesNotThrow(() => OnLoad.SetColorSpace(OnLoad.UnityColorSpace.Linear),
                "SetColorSpace should not throw when called with Linear value");
        }

        [Test]
        public void PlayerColorProperty_ReturnsValidColorSpace()
        {
            // Act - Get the current player color space using reflection for the private property
            var playerColorProperty = typeof(OnLoad).GetProperty("PlayerColorSpace",
                BindingFlags.NonPublic | BindingFlags.Static);
            Assert.IsNotNull(playerColorProperty, "Could not find PlayerColorSpace property.");

            var actualColorSpace = playerColorProperty.GetValue(null);

            // Assert - Should return a valid UnityColorSpace enum value
            Assert.IsNotNull(actualColorSpace, "PlayerColorSpace should not return null");
            Assert.AreEqual(typeof(OnLoad.UnityColorSpace), actualColorSpace.GetType(),
                "PlayerColorSpace should return a UnityColorSpace enum value");

            // Convert to int and verify it's either 0 (Gamma) or 1 (Linear)
            var colorSpaceValue = (int)actualColorSpace;
            Assert.IsTrue(colorSpaceValue == 0 || colorSpaceValue == 1,
                $"PlayerColorSpace should return 0 (Gamma) or 1 (Linear), but got {colorSpaceValue}");
        }

        [Test]
        public void OnLoadClass_HasCorrectNamespace()
        {
            // Assert that the OnLoad class is in the correct namespace
            Assert.AreEqual("LibVLCSharp", typeof(OnLoad).Namespace);
        }

        [Test]
        public void SetColorSpaceMethod_HasCorrectSignature()
        {
            // Get method info using reflection for signature testing
            var setColorSpaceMethod = typeof(OnLoad).GetMethod("SetColorSpace");
            Assert.IsNotNull(setColorSpaceMethod, "Could not find SetColorSpace method.");

            // Assert method signature - SetColorSpace is a P/Invoke method so it has additional attributes
            Assert.IsTrue(setColorSpaceMethod.IsPublic, "SetColorSpace should be public");
            Assert.IsTrue(setColorSpaceMethod.IsStatic, "SetColorSpace should be static");
            Assert.AreEqual("SetColorSpace", setColorSpaceMethod.Name);

            var parameters = setColorSpaceMethod.GetParameters();
            Assert.AreEqual(1, parameters.Length, "SetColorSpace should have one parameter");
            Assert.AreEqual(typeof(OnLoad.UnityColorSpace), parameters[0].ParameterType);
            Assert.AreEqual("colorSpace", parameters[0].Name);
        }
    }
}