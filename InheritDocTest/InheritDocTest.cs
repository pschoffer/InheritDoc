﻿using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using NLog;

using InheritDocLib;

namespace InheritDocTest {
    [TestClass]
    public class InheritDocTest {
        static readonly Logger logger = LogManager.GetCurrentClassLogger();

        [TestMethod]
        public void BasicTest() {
            var basePath = Path.Combine(Environment.CurrentDirectory, @"..\..");
            var newFileNames = InheritDocUtil.Run(basePath: basePath, overwriteExisting: false, logger: Logger);
            var fileName = newFileNames.Where(x => Path.GetFileName(x).StartsWith("InheritDocTest.")).First();
            using (var streamReader = new StreamReader(fileName)) {
                var xDocument = XDocument.Load(streamReader);

                // Check inheritance
                CheckForClassAComments(
                    xDocument, 
                    "ClassA", 
                    rootSummaryValue: "ClassA-Class-Summary",
                    rootRemarksValue: "ClassA-Class-Remarks",
                    propertySummaryValue: "ClassA-MyProperty-Summary",
                    propertyValueValue: "ClassA-MyProperty-Value",
                    methodSummaryValue: "ClassA-MyMethod-Summary"
                );
                CheckForClassAComments(
                    xDocument,
                    "ClassAA"
                );
                CheckForClassAComments(
                    xDocument,
                    "ClassAB",
                    rootSummaryValue: "ClassA-Class-Summary",
                    rootRemarksValue: "ClassA-Class-Remarks",
                    propertySummaryValue: "ClassA-MyProperty-Summary",
                    propertyValueValue: "ClassA-MyProperty-Value",
                    methodSummaryValue: "ClassA-MyMethod-Summary"
                );
                CheckForClassAComments(
                    xDocument,
                    "ClassAC",
                    rootSummaryValue: "ClassA-Class-Summary",
                    rootRemarksValue: "ClassA-Class-Remarks",
                    propertySummaryValue: "ClassA-MyProperty-Summary",
                    propertyValueValue: "ClassA-MyProperty-Value",
                    methodSummaryValue: "ClassA-MyMethod-Summary"
                );
                CheckForClassAComments(
                    xDocument,
                    "ClassAD",
                    propertySummaryValue: "ClassA-MyProperty-Summary",
                    propertyValueValue: "ClassA-MyProperty-Value",
                    methodSummaryValue: "ClassA-MyMethod-Summary"
                );
                CheckForClassAComments(
                    xDocument,
                    "ClassAE",
                    propertySummaryValue: "ClassA-MyProperty-Summary",
                    methodSummaryValue: "ClassA-MyMethod-Summary"
                );
                CheckForClassAComments(
                    xDocument,
                    "ClassAF",
                    propertySummaryValue: "ClassA-MyProperty-Summary",
                    propertyValueValue: "ClassA-MyProperty-Value",
                    methodSummaryValue: "ClassA-MyMethod-Summary"
                );
                CheckForClassAComments(
                    xDocument,
                    "ClassAG",
                    propertySummaryValue: "ClassA-MyProperty-Summary",
                    propertyValueValue: "ClassAG-MyProperty-Value"
                );
                CheckForClassAComments(
                    xDocument,
                    "ClassAH",
                    rootSummaryValue: "ClassA-Class-Summary"
                );
                CheckForClassAComments(
                    xDocument,
                    "ClassAAA",
                    rootSummaryValue: "ClassA-Class-Summary",
                    rootRemarksValue: "ClassA-Class-Remarks",
                    propertySummaryValue: "ClassA-MyProperty-Summary",
                    propertyValueValue: "ClassA-MyProperty-Value",
                    methodSummaryValue: "ClassA-MyMethod-Summary"
                );

                // Test interfaces
                CheckForMethodSummaryComments(
                    xDocument,
                    "ClassBA",
                    "MethodA",
                    "InterfaceA-MethodA-Summary"
                );
                CheckForMethodSummaryComments(
                    xDocument,
                    "ClassBB",
                    "MethodB",
                    "InterfaceB-MethodB-Summary"
                );
                CheckForMethodSummaryComments(
                    xDocument,
                    "ClassBAB",
                    "MethodA",
                    "InterfaceA-MethodA-Summary"
                );
                CheckForMethodSummaryComments(
                    xDocument,
                    "ClassBAB",
                    "MethodB",
                    "InterfaceB-MethodB-Summary"
                );
                CheckForMethodSummaryComments(
                    xDocument,
                    "ClassBC",
                    "MethodB",
                    "InterfaceB-MethodB-Summary"
                );
                CheckForMethodSummaryComments(
                    xDocument,
                    "ClassBC",
                    "MethodC",
                    "InterfaceC-MethodC-Summary"
                );
                CheckForMethodSummaryComments(
                    xDocument,
                    "ClassBA",
                    "MethodA",
                    "InterfaceA-MethodA-Summary"
                );
                CheckForMethodSummaryComments(
                    xDocument,
                    "ClassBA",
                    "MethodA",
                    "InterfaceA-MethodA-Summary"
                );
                CheckForMethodSummaryComments(
                    xDocument,
                    "ClassBA",
                    "MethodA",
                    "InterfaceA-MethodA-Summary"
                );

                // Test cref attribute
                CheckForMethodSummaryComments(
                    xDocument,
                    "ClassCB",
                    "MethodA(System.Int32)",
                    "ClassCA-MethodA(int)-Summary"
                );
                CheckForMethodSummaryComments(
                    xDocument,
                    "ClassCB",
                    "MethodA(System.String)",
                    "ClassCA-MethodA(string)-Summary"
                );
                CheckForMethodSummaryComments(
                    xDocument,
                    "ClassCB",
                    "MethodC",
                    "ClassCA-MethodB-Summary"
                );
                CheckForMethodSummaryComments(
                    xDocument,
                    "ClassCB",
                    "MethodD",
                    "ClassCA-MethodB-Summary"
                );

                CheckForClassSummaryComments(
                    xDocument,
                    "ClassDA",
                    new string[] { "ClassDA-Summary(1)", "ClassDA-Summary(2)" }
                );
                CheckForClassSummaryComments(
                    xDocument,
                    "ClassDB",
                    new string[] { "ClassDA-Summary(1)", "ClassDA-Summary(2)" }
                );
                CheckForClassSummaryComments(
                    xDocument,
                    "ClassDC",
                    new string[] { "ClassDC-Summary" }
                );
                CheckForClassSummaryComments(
                    xDocument,
                    "ClassDD",
                    new string[] { "ClassDC-Summary" }
                );
            }
        }

        [TestMethod]
        public void LibraryTest() {
            var basePath = Path.Combine(Environment.CurrentDirectory, @"..\..");
            var libraryPath = @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.X";
            var newFileNames = InheritDocUtil.Run(basePath: basePath, xmlExtraSearchPath: libraryPath, overwriteExisting: false, logger: Logger);
            var fileName = newFileNames.Where(x => Path.GetFileName(x).StartsWith("InheritDocTest.")).First();
            using (var streamReader = new StreamReader(fileName)) {
                var xDocument = XDocument.Load(streamReader);

                CheckForMethodSummaryComments(
                    xDocument,
                    "MyStream",
                    "Flush",
                    "InterfaceC-MethodC-Summary"
                );
            }
        }

        static void CheckForClassAComments(XDocument xDocument, string className, string rootSummaryValue = null, string rootRemarksValue = null, string propertySummaryValue = null, string propertyValueValue = null, string methodSummaryValue = null) {
            // Check root tags
            var classRoots = xDocument.Descendants("member").Where(x => x.Attribute("name").Value == $"T:InheritDocTest.{className}");
            Assert.AreEqual((!string.IsNullOrEmpty(rootSummaryValue) ? 1 : 0) + (!string.IsNullOrEmpty(rootRemarksValue) ? 1 : 0), classRoots.Count()==0 ? 0 : classRoots.First().Elements().Count());
            if (classRoots.Count()>0) {
                if (!string.IsNullOrEmpty(rootSummaryValue)) Assert.AreEqual(rootSummaryValue, classRoots.First().Descendants("summary").First().Value.Trim());
                if (!string.IsNullOrEmpty(rootRemarksValue)) Assert.AreEqual(rootRemarksValue, classRoots.First().Descendants("remarks").First().Value.Trim());
            }

            // Check MyProperty tags
            var myProperties = xDocument.Descendants("member").Where(x => x.Attribute("name").Value == $"P:InheritDocTest.{className}.MyProperty");
            Assert.AreEqual((!string.IsNullOrEmpty(propertySummaryValue) ? 1 : 0) + (!string.IsNullOrEmpty(propertyValueValue) ? 1 : 0), myProperties.Count()==0 ? 0 : myProperties.First().Elements().Count());
            if (myProperties.Count() > 0) {
                if (!string.IsNullOrEmpty(propertySummaryValue)) Assert.AreEqual(propertySummaryValue, myProperties.First().Descendants("summary").First().Value.Trim());
                if (!string.IsNullOrEmpty(propertyValueValue)) Assert.AreEqual(propertyValueValue, myProperties.First().Descendants("value").First().Value.Trim());
            }

            // Check MyMethod tags
            var myMethods = xDocument.Descendants("member").Where(x => x.Attribute("name").Value == $"M:InheritDocTest.{className}.MyMethod");
            Assert.AreEqual((!string.IsNullOrEmpty(methodSummaryValue) ? 1 : 0), myMethods.Count()==0 ? 0 : myMethods.First().Elements().Count());
            if (myMethods.Count() > 0) {
                if (!string.IsNullOrEmpty(methodSummaryValue)) Assert.AreEqual(methodSummaryValue, myMethods.First().Descendants("summary").First().Value.Trim());
            }
        }

        static void CheckForMethodSummaryComments(XDocument xDocument, string className, string methodName, string commentValue) {
            var name = $"M:InheritDocTest.{className}.{methodName}";
            var myMethods = xDocument.Descendants("member").Where(x => x.Attribute("name").Value == name);
            Assert.AreEqual(string.IsNullOrEmpty(commentValue) ? 0 : 1, myMethods.Count()==0 ? 0 : myMethods.First().Elements().Count());
            if (myMethods.Count() > 0) {
                Assert.AreEqual(commentValue, myMethods.First().Descendants("summary").First().Value.Trim());
            }
        }

        static void CheckForClassSummaryComments(XDocument xDocument, string className, string[] commentValues) {
            var name = $"T:InheritDocTest.{className}";
            var myMethods = xDocument.Descendants("member").Where(x => x.Attribute("name").Value == name);
            var summaryTags = myMethods.First().Descendants("summary");
            Assert.AreEqual(commentValues.Length, summaryTags.Count());
            foreach (var pair in summaryTags.Zip(commentValues, (summaryTag, commentValue) => new { summaryTag, commentValue })) {
                Assert.AreEqual(pair.commentValue, pair.summaryTag.Value.Trim());
            }
        }

        static void Logger(InheritDocLib.LogLevel logLevel, string message) {
            switch (logLevel) {
                case InheritDocLib.LogLevel.Trace:
                    logger.Trace(message);
                    break;
                case InheritDocLib.LogLevel.Debug:
                    logger.Debug(message);
                    break;
                case InheritDocLib.LogLevel.Info:
                    logger.Info(message);
                    break;
                case InheritDocLib.LogLevel.Warn:
                    logger.Warn(message);
                    break;
                case InheritDocLib.LogLevel.Error:
                    logger.Error(message);
                    break;
            }
        }
    }

    /// <summary>
    /// ClassA-Class-Summary
    /// </summary>
    /// <remarks>
    /// ClassA-Class-Remarks
    /// </remarks>
    public class ClassA : IDisposable {
        /// <summary>
        /// ClassA-MyProperty-Summary
        /// </summary>
        /// <value>
        /// ClassA-MyProperty-Value
        /// </value>
        public virtual int MyProperty { get; }

        public void Dispose() {
            throw new NotImplementedException();
        }

        /// <summary>
        /// ClassA-MyMethod-Summary
        /// </summary>
        public virtual void MyMethod() { }
    }

    public class ClassAA : ClassA {
    }

    /// <inheritdoc/>
    public class ClassAB : ClassA {
    }

    /// <inheritdoc/>
    public class ClassAC : ClassA {
        public override int MyProperty => base.MyProperty;

        public override void MyMethod() {
            base.MyMethod();
        }
    }

    public class ClassAD : ClassA {
        /// <inheritdoc/>
        public override int MyProperty => base.MyProperty;

        /// <inheritdoc/>
        public override void MyMethod() {
            base.MyMethod();
        }
    }

    public class ClassAE : ClassA {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override int MyProperty => base.MyProperty;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override void MyMethod() {
            base.MyMethod();
        }
    }

    public class ClassAF : ClassA {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <value>
        /// <inheritdoc/>
        /// </value>
        public override int MyProperty => base.MyProperty;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override void MyMethod() {
            base.MyMethod();
        }
    }

    public class ClassAG : ClassA {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <value>
        /// ClassAG-MyProperty-Value
        /// </value>
        public override int MyProperty => base.MyProperty;

        public override void MyMethod() {
            base.MyMethod();
        }
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public class ClassAH : ClassA {
    }

    /// <inheritdoc/>
    public class ClassAAA : ClassAA {
    }

    /// <summary>
    /// InterfaceA-Interface-Summary
    /// </summary>
    public interface IInterfaceA {
        /// <summary>
        /// InterfaceA-MethodA-Summary
        /// </summary>
        void MethodA();
    }

    /// <summary>
    /// InterfaceB-Interface-Summary
    /// </summary>
    public interface IInterfaceB {
        /// <summary>
        /// InterfaceB-MethodB-Summary
        /// </summary>
        void MethodB();
    }

    public interface IInterfaceC : IInterfaceB {
        /// <summary>
        /// InterfaceC-MethodC-Summary
        /// </summary>
        void MethodC();
    }


    /// <inheritdoc/>
    public class ClassBA : IInterfaceA {
        public void MethodA() {
            throw new NotImplementedException();
        }
    }

    /// <inheritdoc/>
    public class ClassBB : IInterfaceB {
        public void MethodB() {
            throw new NotImplementedException();
        }
    }

    /// <inheritdoc/>
    public class ClassBAB : IInterfaceA, IInterfaceB {
        public void MethodA() {
            throw new NotImplementedException();
        }

        public void MethodB() {
            throw new NotImplementedException();
        }
    }

    /// <inheritdoc/>
    public class ClassBC : IInterfaceC {
        public void MethodB() {
            throw new NotImplementedException();
        }

        public void MethodC() {
            throw new NotImplementedException();
        }
    }


    public class ClassCA {
        /// <summary>
        /// ClassCA-MethodA(int)-Summary
        /// </summary>
        public void MethodA(int x) {

        }

        /// <summary>
        /// ClassCA-MethodA(string)-Summary
        /// </summary>
        public void MethodA(string x) {

        }

        /// <summary>
        /// ClassCA-MethodB-Summary
        /// </summary>
        public void MethodB() {

        }
    }

    public class ClassCB {
        /// <inheritdoc cref="ClassCA"/>
        public void MethodA(int x) {
        }

        /// <inheritdoc cref="ClassCA"/>
        public void MethodA(string x) {
        }

        /// <inheritdoc cref="ClassCA.MethodB"/>
        public void MethodC() {
        }

        /// <inheritdoc cref="ClassCB.MethodC"/>
        public void MethodD() {

        }
    }

    /// <summary>
    /// ClassDA-Summary(1)
    /// </summary>
    /// <summary>
    /// ClassDA-Summary(2)
    /// </summary>
    public class ClassDA {

    }

    /// <inheritdoc/>
    public class ClassDB : ClassDA {

    }

    /// <summary>
    /// ClassDC-Summary
    /// </summary>
    public class ClassDC : ClassDA {

    }

    /// <inheritdoc/>
    public class ClassDD : ClassDC {

    }

    public class MyStream : Stream {
        public override bool CanRead => throw new NotImplementedException();

        public override bool CanSeek => throw new NotImplementedException();

        public override bool CanWrite => throw new NotImplementedException();

        public override long Length => throw new NotImplementedException();

        public override long Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        /// <inheritdoc/>
        public override void Flush() {
            throw new NotImplementedException();
        }

        public override int Read(byte[] buffer, int offset, int count) {
            throw new NotImplementedException();
        }

        public override long Seek(long offset, SeekOrigin origin) {
            throw new NotImplementedException();
        }

        public override void SetLength(long value) {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count) {
            throw new NotImplementedException();
        }
    }
}
