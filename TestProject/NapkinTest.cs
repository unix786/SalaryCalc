using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using Microsoft.Data.Tools.Schema.Sql.UnitTesting;
using Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject
{
    [TestClass()]
    public class NapkinTest : SqlDatabaseTestClass
    {

        public NapkinTest()
        {
            InitializeComponent();
        }

        [TestInitialize()]
        public void TestInitialize()
        {
            base.InitializeTest();
        }
        [TestCleanup()]
        public void TestCleanup()
        {
            base.CleanupTest();
        }

        #region Designer support code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction dbo_t_RecalcSalaryTest_TestAction;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NapkinTest));
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition positionLookupFail;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition positionLookupSuccess;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition insertResult;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition calculationResult1;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition calculationResult2;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.EmptyResultSetCondition deleteResult;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition baseSalary;
            this.dbo_t_RecalcSalaryTestData = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestActions();
            dbo_t_RecalcSalaryTest_TestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
            positionLookupFail = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            positionLookupSuccess = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            insertResult = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition();
            calculationResult1 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            calculationResult2 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            deleteResult = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.EmptyResultSetCondition();
            baseSalary = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
            // 
            // dbo_t_RecalcSalaryTest_TestAction
            // 
            dbo_t_RecalcSalaryTest_TestAction.Conditions.Add(positionLookupFail);
            dbo_t_RecalcSalaryTest_TestAction.Conditions.Add(positionLookupSuccess);
            dbo_t_RecalcSalaryTest_TestAction.Conditions.Add(insertResult);
            dbo_t_RecalcSalaryTest_TestAction.Conditions.Add(calculationResult1);
            dbo_t_RecalcSalaryTest_TestAction.Conditions.Add(calculationResult2);
            dbo_t_RecalcSalaryTest_TestAction.Conditions.Add(deleteResult);
            dbo_t_RecalcSalaryTest_TestAction.Conditions.Add(baseSalary);
            resources.ApplyResources(dbo_t_RecalcSalaryTest_TestAction, "dbo_t_RecalcSalaryTest_TestAction");
            // 
            // dbo_t_RecalcSalaryTestData
            // 
            this.dbo_t_RecalcSalaryTestData.PosttestAction = null;
            this.dbo_t_RecalcSalaryTestData.PretestAction = null;
            this.dbo_t_RecalcSalaryTestData.TestAction = dbo_t_RecalcSalaryTest_TestAction;
            // 
            // positionLookupFail
            // 
            positionLookupFail.ColumnNumber = 1;
            positionLookupFail.Enabled = true;
            positionLookupFail.ExpectedValue = null;
            positionLookupFail.Name = "positionLookupFail";
            positionLookupFail.NullExpected = true;
            positionLookupFail.ResultSet = 1;
            positionLookupFail.RowNumber = 1;
            // 
            // positionLookupSuccess
            // 
            positionLookupSuccess.ColumnNumber = 1;
            positionLookupSuccess.Enabled = true;
            positionLookupSuccess.ExpectedValue = "1";
            positionLookupSuccess.Name = "positionLookupSuccess";
            positionLookupSuccess.NullExpected = false;
            positionLookupSuccess.ResultSet = 2;
            positionLookupSuccess.RowNumber = 1;
            // 
            // insertResult
            // 
            insertResult.Enabled = true;
            insertResult.Name = "insertResult";
            insertResult.ResultSet = 4;
            insertResult.RowCount = 1;
            // 
            // calculationResult1
            // 
            calculationResult1.ColumnNumber = 1;
            calculationResult1.Enabled = true;
            calculationResult1.ExpectedValue = "1380";
            calculationResult1.Name = "calculationResult1";
            calculationResult1.NullExpected = false;
            calculationResult1.ResultSet = 4;
            calculationResult1.RowNumber = 1;
            // 
            // calculationResult2
            // 
            calculationResult2.ColumnNumber = 2;
            calculationResult2.Enabled = true;
            calculationResult2.ExpectedValue = "2180";
            calculationResult2.Name = "calculationResult2";
            calculationResult2.NullExpected = false;
            calculationResult2.ResultSet = 4;
            calculationResult2.RowNumber = 1;
            // 
            // deleteResult
            // 
            deleteResult.Enabled = true;
            deleteResult.Name = "deleteResult";
            deleteResult.ResultSet = 5;
            // 
            // baseSalary
            // 
            baseSalary.ColumnNumber = 1;
            baseSalary.Enabled = true;
            baseSalary.ExpectedValue = "1200";
            baseSalary.Name = "baseSalary";
            baseSalary.NullExpected = false;
            baseSalary.ResultSet = 3;
            baseSalary.RowNumber = 1;
        }

        #endregion


        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        #endregion

        [TestMethod()]
        public void dbo_t_RecalcSalaryTest()
        {
            SqlDatabaseTestActions testActions = this.dbo_t_RecalcSalaryTestData;
            // Execute the pre-test script
            // 
            System.Diagnostics.Trace.WriteLineIf((testActions.PretestAction != null), "Executing pre-test script...");
            SqlExecutionResult[] pretestResults = TestService.Execute(this.PrivilegedContext, this.PrivilegedContext, testActions.PretestAction);
            try
            {
                // Execute the test script
                // 
                System.Diagnostics.Trace.WriteLineIf((testActions.TestAction != null), "Executing test script...");
                SqlExecutionResult[] testResults = TestService.Execute(this.ExecutionContext, this.PrivilegedContext, testActions.TestAction);
            }
            finally
            {
                // Execute the post-test script
                // 
                System.Diagnostics.Trace.WriteLineIf((testActions.PosttestAction != null), "Executing post-test script...");
                SqlExecutionResult[] posttestResults = TestService.Execute(this.PrivilegedContext, this.PrivilegedContext, testActions.PosttestAction);
            }
        }
        private SqlDatabaseTestActions dbo_t_RecalcSalaryTestData;
    }
}
