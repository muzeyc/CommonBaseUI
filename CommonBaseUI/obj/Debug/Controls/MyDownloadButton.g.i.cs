﻿#pragma checksum "..\..\..\Controls\MyDownloadButton.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "F57AFA49E8AC52F6CB3A86F9E723D32F69FDE8FD"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using Util.Controls;


namespace CommonBaseUI.Controls {
    
    
    /// <summary>
    /// MyDownloadButton
    /// </summary>
    public partial class MyDownloadButton : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 9 "..\..\..\Controls\MyDownloadButton.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border pnlBorder;
        
        #line default
        #line hidden
        
        
        #line 10 "..\..\..\Controls\MyDownloadButton.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Util.Controls.FButton btnDownload;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/CommonBaseUI;component/controls/mydownloadbutton.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Controls\MyDownloadButton.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.pnlBorder = ((System.Windows.Controls.Border)(target));
            return;
            case 2:
            this.btnDownload = ((Util.Controls.FButton)(target));
            
            #line 13 "..\..\..\Controls\MyDownloadButton.xaml"
            this.btnDownload.MouseMove += new System.Windows.Input.MouseEventHandler(this.btnUpload_MouseMove);
            
            #line default
            #line hidden
            
            #line 13 "..\..\..\Controls\MyDownloadButton.xaml"
            this.btnDownload.MouseLeave += new System.Windows.Input.MouseEventHandler(this.btnUpload_MouseLeave);
            
            #line default
            #line hidden
            
            #line 14 "..\..\..\Controls\MyDownloadButton.xaml"
            this.btnDownload.Click += new System.Windows.RoutedEventHandler(this.btnDownload_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

