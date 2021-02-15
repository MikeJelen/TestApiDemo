// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "All queries in resources and have been reviewed", Scope = "member", Target = "~M:TestApiDemo.Tests.TestBase`1.ExecuteQuery(System.String)~System.Data.DataSet")]
[assembly: SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "using test to catch any exception so generic is preferred", Scope = "member", Target = "~M:TestApiDemo.Tests.AsyncTests.Delete")]
[assembly: SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "using test to catch any exception so generic is preferred", Scope = "member", Target = "~M:TestApiDemo.Tests.AsyncTests.Post")]
[assembly: SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "using test to catch any exception so generic is preferred", Scope = "member", Target = "~M:TestApiDemo.Tests.TestBase`1.CleanMessageQueue")]
