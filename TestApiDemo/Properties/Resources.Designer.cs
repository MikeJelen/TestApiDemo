﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TestApiDemo.Properties {
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("TestApiDemo.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to Test-Group.
        /// </summary>
        internal static string MessageGroupId {
            get {
                return ResourceManager.GetString("MessageGroupId", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to localhost:9092.
        /// </summary>
        internal static string MessageServerUri {
            get {
                return ResourceManager.GetString("MessageServerUri", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Inventory.
        /// </summary>
        internal static string MessageTopic {
            get {
                return ResourceManager.GetString("MessageTopic", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Server=(localdb)\ProjectsV13;Database=MikeDemo;Trusted_Connection=True;.
        /// </summary>
        internal static string SqlConnection {
            get {
                return ResourceManager.GetString("SqlConnection", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to [dbo].[sp_DeleteProduct].
        /// </summary>
        internal static string StoredProcedure_DeleteProduct {
            get {
                return ResourceManager.GetString("StoredProcedure_DeleteProduct", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to [dbo].[sp_UpsertProduct].
        /// </summary>
        internal static string StoredProcedure_UpsertProduct {
            get {
                return ResourceManager.GetString("StoredProcedure_UpsertProduct", resourceCulture);
            }
        }
    }
}
