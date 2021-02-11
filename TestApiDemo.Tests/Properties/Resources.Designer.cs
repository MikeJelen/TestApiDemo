﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TestApiDemo.Tests.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("TestApiDemo.Tests.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to declare @i int = (select ProductId from dbo.Product where Name = &apos;&lt;@Name&gt;&apos;); 
        ///delete from dbo.ProductInventory where ProductId = @i; 
        ///delete from dbo.Product where ProductId = @i; .
        /// </summary>
        internal static string DeleteProduct {
            get {
                return ResourceManager.GetString("DeleteProduct", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to select	  p.[Name] as &apos;Name&apos;, isnull(i.[Quantity], 0) as &apos;Quantity&apos;, replace(rtrim(replace(convert(varchar(50), p.[CreatedOn], 126),&apos;0&apos;,&apos; &apos;)),&apos; &apos;,&apos;0&apos;) as &apos;CreatedOn&apos; from dbo.Product p left outer join dbo.ProductInventory i on p.ProductId = i.ProductId for json path;.
        /// </summary>
        internal static string Get {
            get {
                return ResourceManager.GetString("Get", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to select	  p.[Name] as &apos;Name&apos;, isnull(i.[Quantity], 0) as &apos;Quantity&apos;, replace(rtrim(replace(convert(varchar(50), p.[CreatedOn], 126),&apos;0&apos;,&apos; &apos;)),&apos; &apos;,&apos;0&apos;) as &apos;CreatedOn&apos; from dbo.Product p left outer join dbo.ProductInventory i on p.ProductId = i.ProductId where p.[Name] = &apos;&lt;@Name&gt;&apos; for json path;.
        /// </summary>
        internal static string GetByName {
            get {
                return ResourceManager.GetString("GetByName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to select top 1 p.[Name] from dbo.Product p;.
        /// </summary>
        internal static string GetFirstProduct {
            get {
                return ResourceManager.GetString("GetFirstProduct", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to declare @table table ([Name] varchar(200) primary key, [Quantity] int, [CreatedOn] varchar(50));
        ///insert into @table
        ///select p.[Name], isnull(i.[Quantity], 0), replace(rtrim(replace(convert(varchar(50), p.[CreatedOn], 126),&apos;0&apos;,&apos; &apos;)),&apos; &apos;,&apos;0&apos;) from dbo.Product p left outer join dbo.ProductInventory i on p.ProductId = i.ProductId;
        ///declare @max int = (select max(Quantity) from @table);
        ///select	* from	@table where Quantity = @max for json path;.
        /// </summary>
        internal static string GetHighestQuantity {
            get {
                return ResourceManager.GetString("GetHighestQuantity", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to select top 1 p.[Name] from dbo.Product p inner join dbo.ProductInventory i on p.[ProductId] = i.[ProductId] order by p.[CreatedOn] desc;.
        /// </summary>
        internal static string GetLastProduct {
            get {
                return ResourceManager.GetString("GetLastProduct", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to declare @table table ([Name] varchar(200) primary key, [Quantity] int, [CreatedOn] varchar(50));
        ///
        ///insert into @table
        ///select  p.[Name], isnull(i.[Quantity], 0), replace(rtrim(replace(convert(varchar(50), p.[CreatedOn], 126),&apos;0&apos;,&apos; &apos;)),&apos; &apos;,&apos;0&apos;) from dbo.Product p left outer join dbo.ProductInventory i on p.ProductId = i.ProductId;
        ///declare @min int = (select min(Quantity) from @table);
        ///select * from @table where Quantity = @min for json path;.
        /// </summary>
        internal static string GetLowestQuantity {
            get {
                return ResourceManager.GetString("GetLowestQuantity", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to declare @max datetime2 = (select max([CreatedOn]) from [dbo].[Product]);
        ///
        ///select p.[Name] as &apos;Name&apos;, isnull(i.[Quantity], 0)	as &apos;Quantity&apos;, replace(rtrim(replace(convert(varchar(50), p.[CreatedOn], 126),&apos;0&apos;,&apos; &apos;)),&apos; &apos;,&apos;0&apos;) as &apos;CreatedOn&apos; from dbo.Product p left outer join dbo.ProductInventory i on p.ProductId = i.ProductId where p.[CreatedOn] = @max for json path;.
        /// </summary>
        internal static string GetNewest {
            get {
                return ResourceManager.GetString("GetNewest", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to declare @min datetime2 = (select min([CreatedOn]) from [dbo].[Product]);
        ///
        ///select p.[Name] as &apos;Name&apos;, isnull(i.[Quantity], 0)	as &apos;Quantity&apos;, replace(rtrim(replace(convert(varchar(50), p.[CreatedOn], 126),&apos;0&apos;,&apos; &apos;)),&apos; &apos;,&apos;0&apos;) as &apos;CreatedOn&apos; from dbo.Product p left outer join dbo.ProductInventory i on p.ProductId = i.ProductId where p.[CreatedOn] = @min for json path;.
        /// </summary>
        internal static string GetOldest {
            get {
                return ResourceManager.GetString("GetOldest", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to insert into dbo.Product (Name) values (&apos;&lt;@Name&gt;&apos;);
        ///declare @i int = scope_identity();
        ///insert into dbo.ProductInventory (ProductId, Quantity) values (@i, 50);.
        /// </summary>
        internal static string InsertProduct {
            get {
                return ResourceManager.GetString("InsertProduct", resourceCulture);
            }
        }
    }
}
