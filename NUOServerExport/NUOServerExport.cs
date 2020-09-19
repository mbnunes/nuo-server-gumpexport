namespace NUOServerExport
{
    using GumpStudio;
    using GumpStudio.Elements;
    using GumpStudio.Plugins;
    using System;
    using System.Collections;
    using System.IO;
    using System.Windows.Forms;

    public class NUOServerExport : GumpStudio.Plugins.BasePlugin
    {
        private GumpStudio.DesignerForm designer;
        protected NUOServerExportForm frmNUOServer;
        private MenuItem menuitem;
        private string header = "import { IPlayer } from \"../../server/mobiles/players/Player\";\nimport Packet_0xB1 from \"../../server/networks/packets/client/0xB1\";\nimport Server from \"../../server/Server\";\nimport Gump from \"../Gump\";"; 


        public override GumpStudio.Plugins.PluginInfo GetPluginInfo() => 
            new GumpStudio.Plugins.PluginInfo { 
                AuthorEmail = "mutila@gmail.com",
                AuthorName = "Mauricio Nunes",
                Description = "Allows the user to export a gump to a NUO-Server compatible script, based in NUOServerGumpExporter.",
                PluginName = "NUO-Server",
                Version = "1.0"
            };

        public override void Load(GumpStudio.DesignerForm designer)
        {
            this.designer = designer;
            this.menuitem = new MenuItem("NUO-Server");
            this.menuitem.Click += new EventHandler(this.menuitem_Click);
            if (!this.designer.mnuFileExport.Enabled)
            {
                this.designer.mnuFileExport.Enabled = true;
            }
            designer.mnuFileExport.MenuItems.Add(this.menuitem);
            base.Load(designer);
        }

        private void menuitem_Click(object sender, EventArgs e)
        {
            this.frmNUOServer = new NUOServerExportForm(this);
            this.frmNUOServer.ShowDialog();
        }

        public void OnExportNUOServer(object sender, EventArgs eventargs, int x, int y, bool noDispose, bool noClose, bool noMove, string gumpName)
        {
            SaveFileDialog dialog = new SaveFileDialog {
                Filter = "Script File (*.ts)|*.ts"
            };
            int group = -1;
            if (dialog.ShowDialog(this.designer) != DialogResult.Cancel)
            {
                StreamWriter writer = File.CreateText(dialog.FileName);
                writer.WriteLine(header);
                writer.WriteLine("export class "+gumpName+" extends Gump {");
                writer.WriteLine("\tconstructor(player: IPlayer, server: Server) {");
                writer.WriteLine("\t\tsuper(player, server);");

                writer.WriteLine("");

                writer.WriteLine("\t\tthis.noClose = "+noClose+";");
                writer.WriteLine("\t\tthis.noDispose = " + noDispose + ";");
                writer.WriteLine("\t\this.noMove =" + noMove + ";");

                int num = 0;
                int pageIndex = 0;
                if (0 < this.designer.Stacks.Count)
                {
                    do
                    {
                        if (pageIndex > 0)
                            writer.Write("");

                        GumpStudio.Elements.GroupElement element12 = this.designer.Stacks[num] as GumpStudio.Elements.GroupElement;
                        writer.Write("\n\t\tthis.addPage(" +num+ ");");
                        if (element12 != null)
                        {
                            ArrayList elementsRecursive = element12.GetElementsRecursive();
                            int num2 = 0;
                            if (0 < elementsRecursive.Count)
                            {
                                do
                                {
                                    GumpStudio.Elements.BaseElement element = elementsRecursive[num2] as GumpStudio.Elements.BaseElement;
                                    string name = element.GetType().Name;

                                    GumpStudio.Elements.AlphaElement element10 = element as GumpStudio.Elements.AlphaElement;
                                    if (element10 != null)
                                    {
                                        writer.Write("\n\t\tthis.addTransparency(" + element10.X+ ", "+ element10.Y + ", "+ element10.Width + ", "+ element10.Height + ");");
                                    }
                                    else
                                    {
                                        GumpStudio.Elements.BackgroundElement element8 = element as GumpStudio.Elements.BackgroundElement;
                                        if (element8 != null)
                                        {
                                            writer.Write("\n\t\tthis.addBackground(" + element8.X + ", "+ element8.Y + ", "+ element8.Width + ", "+ element8.Height + ", "+ element8.GumpID +");");
                                        }
                                        else
                                        {
                                            GumpStudio.Elements.ButtonElement element4 = element as GumpStudio.Elements.ButtonElement;
                                            if (element4 != null)
                                            {
                                                if (element4.ButtonType == GumpStudio.Elements.ButtonTypeEnum.Reply)
                                                {
                                                    writer.Write("\n\t\tthis.addButton(" + element4.X+", "+ element4.Y + ", "+ element4.NormalID + ", "+ element4.PressedID +  ", 1, " + element4.Z + ", " + element4.Param + ");");
                                                }
                                                else if (element4.ButtonType == GumpStudio.Elements.ButtonTypeEnum.Page)
                                                {
                                                    writer.Write("\n\t\tthis.addButton(" + element4.X + ", " + element4.Y + ", " + element4.NormalID + ", " + element4.PressedID + ", 0, " + element4.Param + ", " + element4.Z + ");");
                                                }
                                            }
                                            else
                                            {
                                                GumpStudio.Elements.CheckboxElement element6 = element as GumpStudio.Elements.CheckboxElement;
                                                if (element6 != null)
                                                {
                                                    writer.Write("\n\t\tthis.addCheckbox(" + element6.X+", "+ element6.Y + ", "+ element6.UnCheckedID + ", "+ element6.CheckedID + ", "+ element6.Group + ", "+ (element6.Checked ? 1 : 0) + "); ");
                                                }
                                                else
                                                {
                                                    GumpStudio.Elements.ImageElement element9 = element as GumpStudio.Elements.ImageElement;
                                                    if (element9 != null)
                                                    {
                                                        writer.Write("\n\t\tthis.addGumpPic(" + element9.X + ", "+ element9.Y + ", "+ element9.GumpID + ", "+ element9.Hue.Index + ");");
                                                    }
                                                    else
                                                    {
                                                        GumpStudio.Elements.ItemElement element11 = element as GumpStudio.Elements.ItemElement;
                                                        if (element11 != null)
                                                        {
                                                            writer.Write("\n\t\tthis.addTilePic("+ element11.X+", "+ element11.Y + ", "+ element11.ItemID +");");
                                                        }
                                                        else
                                                        {
                                                            GumpStudio.Elements.LabelElement element7 = element as GumpStudio.Elements.LabelElement;
                                                            if (element7 != null)
                                                            {
                                                                writer.Write("\n\t\tthis.addText(");
                                                                writer.Write(element7.X);
                                                                writer.Write(", ");
                                                                writer.Write(element7.Y);
                                                                writer.Write(", ");
                                                                writer.Write(element7.Hue.Index);
                                                                writer.Write(", \"");
                                                                if (element7.Text != null)
                                                                {
                                                                    writer.Write(element7.Text.Replace("\"", "\\\""));
                                                                }                                                              
                                                                writer.WriteLine("\");");
                                                            }
                                                            else
                                                            {
                                                                GumpStudio.Elements.RadioElement element2 = element as GumpStudio.Elements.RadioElement;
                                                                if (element2 != null)
                                                                {
                                                                    if (element2.Group != group)
                                                                    {
                                                                        writer.Write("\n\t\tthis.addGroup(");
                                                                        writer.Write(element2.Group);
                                                                        writer.WriteLine(");");
                                                                        group = element2.Group;
                                                                    }
                                                                    writer.Write("\n\t\tthis.addRadio(");
                                                                    writer.Write(element2.X);
                                                                    writer.Write(", ");
                                                                    writer.Write(element2.Y);
                                                                    writer.Write(", ");
                                                                    writer.Write(element2.UnCheckedID);
                                                                    writer.Write(", ");
                                                                    writer.Write(element2.CheckedID);
                                                                    writer.Write(", ");
                                                                    writer.Write(element2.Value);
                                                                    writer.Write(", ");
                                                                    writer.Write(element2.Checked ? 1 : 0);
                                                                    writer.WriteLine(");");
                                                                }
                                                                else
                                                                {
                                                                    GumpStudio.Elements.TextEntryElement element3 = element as GumpStudio.Elements.TextEntryElement;
                                                                    if (element3 != null)
                                                                    {
                                                                        writer.Write("\n\t\tthis.addTextEntry(");
                                                                        writer.Write(element3.X);
                                                                        writer.Write(", ");
                                                                        writer.Write(element3.Y);
                                                                        writer.Write(", ");
                                                                        writer.Write(element3.Width);
                                                                        writer.Write(", ");
                                                                        writer.Write(element3.Height);
                                                                        writer.Write(", ");
                                                                        writer.Write(element3.Hue.Index);
                                                                        writer.Write(", ");
                                                                        writer.Write(element3.ID);
                                                                        writer.Write(", \"");
                                                                        if (element3.InitialText != null)
                                                                        {
                                                                            writer.Write(element3.InitialText.Replace("\"", "\\\""));
                                                                        }
                                                                        writer.WriteLine("\");");
                                                                    }
                                                                    else
                                                                    {
                                                                        GumpStudio.Elements.TiledElement element5 = element as GumpStudio.Elements.TiledElement;
                                                                        if (element5 != null)
                                                                        {
                                                                            writer.Write("\n\t\tthis.addBackground(");
                                                                            writer.Write(element5.X);
                                                                            writer.Write(", ");
                                                                            writer.Write(element5.Y);
                                                                            writer.Write(", ");
                                                                            writer.Write(element5.Width);
                                                                            writer.Write(", ");
                                                                            writer.Write(element5.Height);
                                                                            writer.Write(", ");
                                                                            writer.Write(element5.GumpID);
                                                                            writer.Write(", ");
                                                                            writer.Write(element5.Hue.Index);
                                                                            writer.WriteLine(");");
                                                                        }
                                                                        else
                                                                        {
                                                                            GumpStudio.Elements.HTMLElement element13 = element as GumpStudio.Elements.HTMLElement;
                                                                            if(element13 != null)
                                                                            {
                                                                                writer.Write("\n\t\tthis.addHtmlGump(" + element13.X + ", " + element13.Y + ", " + element13.Width + ", " + element13.Height + ", \"" + element13.HTML +"\", "+element13.ShowBackground+", " +element13.ShowScrollbar+");");
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    num2++;
                                }
                                while (num2 < elementsRecursive.Count);
                            }
                        }
                        writer.WriteLine("");
                        num++;
                        pageIndex++;
                    }
                    while (num < this.designer.Stacks.Count);
                }
                writer.WriteLine("\t\t}\n\t}\n}");
                writer.Close();
            }
        }

        public override string Name =>
            this.GetPluginInfo().PluginName;
    }
}

