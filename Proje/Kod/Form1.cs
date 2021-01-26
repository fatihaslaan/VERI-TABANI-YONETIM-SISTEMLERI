using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace VTYSP
{
    public partial class Form1 : Form
    {
        private List<int> selectedquestionids=new List<int>();
        private int currentquestionid;
        private int currentpersonid;
        bool vote = true;
        bool user = false;
        bool admin = false;
        public Form1()
        {
            InitializeComponent();
            baglanti.Open();
            SqlCommand locs = new SqlCommand("getalllocations", baglanti);
            locs.CommandType = CommandType.StoredProcedure;
            SqlDataReader dr = locs.ExecuteReader();
            while(dr.Read())
            {
                cmb_locations.Items.Add(dr.GetString(0));
            }
            SqlCommand tags = new SqlCommand("getalltags", baglanti);
            tags.CommandType = CommandType.StoredProcedure;
            dr = tags.ExecuteReader();
            while (dr.Read())
            {
                cmb_tags.Items.Add(dr.GetString(0));
            }

        }
        SqlConnection baglanti = new SqlConnection("Data Source=localhost\\SQLEXPRESS;Initial Catalog=Bisorumvar;Integrated Security=True;MultipleActiveResultSets=true");

        void tagyukle()
        {
            if (currentpersonid != 0)
            {
                SqlCommand cmd = new SqlCommand("tagcagir", baglanti);
                SqlDataReader dr = cmd.ExecuteReader();
                AutoCompleteStringCollection tagcomplete = new AutoCompleteStringCollection();
                while (dr.Read())
                {
                    tagcomplete.Add(dr.GetString(0));
                }
                txt_tags.AutoCompleteCustomSource = tagcomplete;
            }
        }

        private void btn_baglantikontrol_Click(object sender, EventArgs e)
        {
            MessageBox.Show(baglanti.State.ToString());
        }

        private void btn_tagara_Click(object sender, EventArgs e)
        {
            if (currentpersonid != 0&&user)
            {
                questlerireset();
                int c = 0;
                SqlCommand tagcom = new SqlCommand("tagidbul", baglanti);
                tagcom.CommandType = CommandType.StoredProcedure;
                tagcom.Parameters.Add("@tag_name", SqlDbType.VarChar, 50).Value = txt_tags.Text;
                SqlDataReader dr = tagcom.ExecuteReader();
                while (dr.Read())
                {
                    c = dr.GetInt32(0);
                }
                SqlCommand questiontitlecom = new SqlCommand("questiontitlebul", baglanti);
                questiontitlecom.CommandType = CommandType.StoredProcedure;
                questiontitlecom.Parameters.Add("@tag_id", SqlDbType.Int).Value = c;
                dr = questiontitlecom.ExecuteReader();
                while (dr.Read())
                {
                    selectedquestionids.Add(dr.GetInt32(0));
                }
                questionlarilistele(selectedquestionids);
            }
        }

        private void questreset()
        {
            lv_dataread.Items.Clear();
            lbl_votedown.Text = "";
            lbl_voteup.Text = "";
        }

        private void questlerireset()
        {
            lv_questions.Items.Clear();
        }

        private void questionlarilistele(List<int> ids)
        {
            for (int i = 0; i < ids.Count; i++)
            {
                SqlCommand questioncom = new SqlCommand("questionagir", baglanti);
                questioncom.CommandType = CommandType.StoredProcedure;
                questioncom.Parameters.Add("@questiontitle_id", SqlDbType.Int).Value = ids[i];
                SqlDataReader dr = questioncom.ExecuteReader();
                while (dr.Read())
                {
                    ListViewItem listViewItem = new ListViewItem(dr.GetString(1));
                    lv_questions.Items.Add(listViewItem);
                }
            }
        }

        private void get5favs()
        {
            if (currentpersonid != 0)
            {
                int j = 0;
                SqlCommand getfavs = new SqlCommand("getfavs", baglanti);
                getfavs.CommandType = CommandType.StoredProcedure;
                getfavs.Parameters.Add("@personid", SqlDbType.Int).Value = currentpersonid;
                SqlDataReader dr = getfavs.ExecuteReader();
                while (dr.Read())
                {
                    if (j < 5)
                    {
                        SqlCommand questioncom = new SqlCommand("questionagir", baglanti);
                        questioncom.CommandType = CommandType.StoredProcedure;
                        questioncom.Parameters.Add("@questiontitle_id", SqlDbType.Int).Value = dr.GetInt32(0);
                        SqlDataReader dr2 = questioncom.ExecuteReader();
                        while (dr2.Read())
                        {
                            ListViewItem listViewItem = new ListViewItem(dr2.GetString(2));
                            lv_favs.Items.Add(listViewItem);
                        }                        
                        j++;
                    }
                }
            }
        }

        private void questionlistele()
        {
            if (currentpersonid != 0)
            {
                questlerireset();
                int authid=0;
                string[] table=new string[2];
                SqlCommand questioncom = new SqlCommand("questionagir", baglanti);
                questioncom.CommandType = CommandType.StoredProcedure;
                questioncom.Parameters.Add("@questiontitle_id", SqlDbType.Int).Value = currentquestionid;
                SqlDataReader dr = questioncom.ExecuteReader();
                while (dr.Read())
                {
                    lbl_questiontitle.Text = "Question: " + dr.GetString(1);
                    authid = dr.GetInt32(2);
                }
                SqlCommand getauth = new SqlCommand("authorbul", baglanti);
                getauth.CommandType = CommandType.StoredProcedure;
                getauth.Parameters.Add("@personid", SqlDbType.Int).Value = authid;
                dr = getauth.ExecuteReader();
                while (dr.Read())
                {
                    table[0] = dr.GetString(0);
                }
                SqlCommand getquest = new SqlCommand("getquestionval", baglanti);
                getquest.CommandType = CommandType.StoredProcedure;
                getquest.Parameters.Add("@questiontitleid", SqlDbType.Int).Value = currentquestionid;
                dr = getquest.ExecuteReader();                
                while (dr.Read())
                {
                    table[1] = dr.GetString(0);
                }
                ListViewItem listViewItem = new ListViewItem(table);
                lv_dataread.Items.Add(listViewItem);
                SqlCommand getanswers = new SqlCommand("allanswers", baglanti);
                getanswers.CommandType = CommandType.StoredProcedure;
                getanswers.Parameters.Add("@questiontitleid", SqlDbType.Int).Value = currentquestionid;
                dr = getanswers.ExecuteReader();
                while(dr.Read())
                {
                    getauth = new SqlCommand("authorbul", baglanti);
                    getauth.CommandType = CommandType.StoredProcedure;
                    getauth.Parameters.Add("@personid", SqlDbType.Int).Value = dr.GetInt32(1);
                    SqlDataReader dr2 = getauth.ExecuteReader();
                    while(dr2.Read())
                    {
                        table[0] = dr2.GetString(0);
                    }
                    table[1] = dr.GetString(2);
                    listViewItem = new ListViewItem(table);
                    lv_dataread.Items.Add(listViewItem);
                }
                SqlCommand getvoteup= new SqlCommand("getvoteup", baglanti);
                getvoteup.CommandType = CommandType.StoredProcedure;
                getvoteup.Parameters.Add("@questiontitleid", SqlDbType.Int).Value = currentquestionid;
                dr = getvoteup.ExecuteReader();
                while (dr.Read())
                {
                    lbl_voteup.Text=""+dr.GetInt32(0);
                }
                SqlCommand getvotedown = new SqlCommand("getvotedown", baglanti);
                getvotedown.CommandType = CommandType.StoredProcedure;
                getvotedown.Parameters.Add("@questiontitleid", SqlDbType.Int).Value = currentquestionid;
                dr = getvotedown.ExecuteReader();
                while (dr.Read())
                {
                    lbl_votedown.Text =""+dr.GetInt32(0);
                }

            }
        }

        private void btn_addtofav_Click(object sender, EventArgs e)
        {
            if(currentpersonid!=0&&currentquestionid!=0)
            {
                SqlCommand addfav = new SqlCommand("addfav", baglanti);
                addfav.CommandType = CommandType.StoredProcedure;
                addfav.Parameters.Add("@questid", SqlDbType.Int).Value = currentquestionid;
                addfav.Parameters.Add("@personid", SqlDbType.Int).Value = currentpersonid;
                addfav.ExecuteNonQuery();
                get5favs();
            }
        }

        private void btn_giris_Click(object sender, EventArgs e)
        {
            if (currentpersonid == 0)
            {
                SqlCommand login = new SqlCommand("u_login", baglanti);
                login.CommandType = CommandType.StoredProcedure;
                login.Parameters.Add("@u_mail", SqlDbType.Text).Value = txt_mail.Text;
                login.Parameters.Add("@u_password", SqlDbType.Text).Value = txt_sifre.Text;
                SqlDataReader dr = login.ExecuteReader();
                while (dr.Read())
                {
                    currentpersonid = dr.GetInt32(0);
                    lbl_username.Text = dr.GetString(2);
                    lbl_usersurname.Text = dr.GetString(3);
                    lbl_usermail.Text = dr.GetString(4);
                    SqlCommand getlocation = new SqlCommand("getlocation", baglanti);
                    getlocation.CommandType = CommandType.StoredProcedure;
                    getlocation.Parameters.Add("@locationid", SqlDbType.Int).Value = dr.GetInt32(1);
                    SqlDataReader dr2 = getlocation.ExecuteReader();
                    while (dr2.Read())
                    {
                        lbl_location.Text = dr2.GetString(0);
                    }
                }
                if (currentpersonid != 0)
                {
                    tagyukle();
                    get5favs();
                    get5quest();
                    tumquestyukle();
                    SqlCommand adminmi = new SqlCommand("adminmi", baglanti);
                    adminmi.CommandType = CommandType.StoredProcedure;
                    adminmi.Parameters.Add("@personid", SqlDbType.Int).Value = currentpersonid;
                    SqlDataReader dr2 = adminmi.ExecuteReader();
                    while (dr2.Read())
                    {
                        admin = true;
                    }
                    SqlCommand usermi = new SqlCommand("usermi", baglanti);
                    usermi.CommandType = CommandType.StoredProcedure;
                    usermi.Parameters.Add("@personid", SqlDbType.Int).Value = currentpersonid;
                    dr2 = usermi.ExecuteReader();
                    while (dr2.Read())
                    {
                        user = true;
                    }
                }
            }
        }

        private void get5quest()
        {
            if (currentpersonid != 0)
            {
                int j = 0;
                SqlCommand getmyquests = new SqlCommand("questlerimibul", baglanti);
                getmyquests.CommandType = CommandType.StoredProcedure;
                getmyquests.Parameters.Add("@personid", SqlDbType.Int).Value = currentpersonid;
                SqlDataReader dr = getmyquests.ExecuteReader();
                while (dr.Read())
                {
                    if (j < 5)
                    {
                        ListViewItem listViewItem = new ListViewItem(dr.GetString(0));
                        lv_5quest.Items.Add(listViewItem);
                        j++;
                    }
                }
            }
        }

        private void tumquestyukle()
        {
            if (currentpersonid != 0)
            {
                int j = 0;
                SqlCommand getallquests = new SqlCommand("tumquestler", baglanti);
                getallquests.CommandType = CommandType.StoredProcedure;
                SqlDataReader dr = getallquests.ExecuteReader();
                while (dr.Read())
                {
                    if (j < 20)
                    {
                        ListViewItem listViewItem = new ListViewItem(dr.GetString(0));
                        lv_questions.Items.Add(listViewItem);
                        j++;
                    }
                }
            }
        }

        private void btn_cikis_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void listView1_ItemActivate(object sender, EventArgs e)
        {
            questreset();
            vote = true;
            ListViewItem item = ((ListView)sender).SelectedItems[0];
            SqlCommand quni = new SqlCommand("questionnamedenidbul", baglanti);
            quni.CommandType = CommandType.StoredProcedure;
            quni.Parameters.Add("@questionname", SqlDbType.Text).Value = item.Text;
            SqlDataReader dr = quni.ExecuteReader();
            while (dr.Read())
            {
                currentquestionid = dr.GetInt32(0);
            }
            questionlistele();
        }

        private void lv_favs_ItemActivate(object sender, EventArgs e)
        {
            questreset();
            vote = true;
            ListViewItem item = ((ListView)sender).SelectedItems[0];
            SqlCommand quni = new SqlCommand("questionnamedenidbul", baglanti);
            quni.CommandType = CommandType.StoredProcedure;
            quni.Parameters.Add("@questionname", SqlDbType.Text).Value = item.Text;
            SqlDataReader dr = quni.ExecuteReader();
            while (dr.Read())
            {
                currentquestionid = dr.GetInt32(0);
            }
            questionlistele();
        }

        private void lv_5quest_ItemActivate(object sender, EventArgs e)
        {
            questreset();
            vote = true;
            ListViewItem item = ((ListView)sender).SelectedItems[0];
            SqlCommand quni = new SqlCommand("questionnamedenidbul", baglanti);
            quni.CommandType = CommandType.StoredProcedure;
            quni.Parameters.Add("@questionname", SqlDbType.Text).Value = item.Text;
            SqlDataReader dr = quni.ExecuteReader();
            while (dr.Read())
            {
                currentquestionid = dr.GetInt32(0);
            }
            questionlistele();
        }

        private void btn_yenile_Click(object sender, EventArgs e)
        {
            if (currentpersonid != 0)
            {
                questreset();
                questlerireset();
                tumquestyukle();
                tagyukle();
                get5favs();
                get5quest();
            }
        }

        private void btn_kayit_Click(object sender, EventArgs e)
        {
            if(txt_isim.Text!=null&&txt_mail.Text!=null&&txt_soyisim.Text!=null&&txt_sifre.Text!=null&&cmb_locations.SelectedText!=null&&currentpersonid==0)
            {
                bool yokmu = true;
                SqlCommand mailvarmi = new SqlCommand("mailvarmibak", baglanti);
                mailvarmi.CommandType = CommandType.StoredProcedure;
                mailvarmi.Parameters.Add("@mail", SqlDbType.Text).Value = txt_mail.Text;
                SqlDataReader dr = mailvarmi.ExecuteReader();
                while (dr.Read())
                {
                    MessageBox.Show("Zaten var");
                    yokmu = false;
                }
                if (yokmu)
                {
                    SqlCommand kayit = new SqlCommand("kayit", baglanti);
                    kayit.CommandType = CommandType.StoredProcedure;
                    kayit.Parameters.Add("@personname", SqlDbType.Text).Value = txt_isim.Text;
                    kayit.Parameters.Add("@personsurname", SqlDbType.Text).Value = txt_soyisim.Text;
                    kayit.Parameters.Add("@personmail", SqlDbType.Text).Value = txt_mail.Text;
                    kayit.Parameters.Add("@personpassword", SqlDbType.Text).Value = txt_sifre.Text;                  
                    kayit.Parameters.Add("@locationid", SqlDbType.Int).Value = cmb_locations.SelectedIndex + 1;
                    kayit.Parameters.Add("@normaluser", SqlDbType.Int).Value = chkbx_user.Checked;
                    kayit.Parameters.Add("@admin", SqlDbType.Int).Value = chkbx_admin.Checked;
                    kayit.ExecuteNonQuery();                
                    btn_giris_Click(sender, e);
                }
            }
        }

        private void btn_askquestion_Click(object sender, EventArgs e)
        {
            if(txt_questiontitle.Text!=null&&txt_question.Text!=null&&cmb_tags.SelectedText!=null&&admin)
            {
                SqlCommand addquest = new SqlCommand("addquestion", baglanti);
                addquest.CommandType = CommandType.StoredProcedure;
                addquest.Parameters.Add("@val", SqlDbType.Text).Value = txt_questiontitle.Text;
                addquest.Parameters.Add("@personid", SqlDbType.Int).Value = currentpersonid;
                addquest.Parameters.Add("@tagid", SqlDbType.Int).Value = cmb_tags.SelectedIndex+1;
                addquest.ExecuteNonQuery();
                int a = 0;
                SqlCommand quni = new SqlCommand("questionnamedenidbul", baglanti);
                quni.CommandType = CommandType.StoredProcedure;
                quni.Parameters.Add("@questionname", SqlDbType.Text).Value = txt_questiontitle.Text;
                SqlDataReader dr = quni.ExecuteReader();
                while (dr.Read())
                {
                    a = dr.GetInt32(0);
                }
                SqlCommand questeval = new SqlCommand("questidyevalekle", baglanti);
                questeval.CommandType = CommandType.StoredProcedure;
                questeval.Parameters.Add("@questid", SqlDbType.Int).Value = a;
                questeval.Parameters.Add("@val", SqlDbType.Text).Value = txt_question.Text;
                questeval.ExecuteNonQuery();
                btn_yenile_Click(sender, e);
            }            
        }

        private void btn_voteup_Click(object sender, EventArgs e)
        {
            if (user &&vote&& currentquestionid != 0)
            {
                SqlCommand voteu = new SqlCommand("voteu", baglanti);
                vote = false;
                voteu.CommandType = CommandType.StoredProcedure;
                voteu.Parameters.Add("@questid", SqlDbType.Int).Value = currentquestionid;
                voteu.ExecuteNonQuery();
                questionlistele();
            }
        }

        private void btn_votedown_Click(object sender, EventArgs e)
        {
            if (user &&vote&& currentquestionid != 0)
            {
                SqlCommand voted = new SqlCommand("voted", baglanti);
                vote = false;
                voted.CommandType = CommandType.StoredProcedure;
                voted.Parameters.Add("@questid", SqlDbType.Int).Value = currentquestionid;
                voted.ExecuteNonQuery();
                questionlistele();
            }
        }

        private void btn_ekle_Click(object sender, EventArgs e)
        {
            if (user && currentquestionid != 0 && txt_answer.Text != null)
            {
                btn_yenile_Click(sender, e);
                SqlCommand addanswer = new SqlCommand("addanswer", baglanti);
                addanswer.CommandType = CommandType.StoredProcedure;
                addanswer.Parameters.Add("@val", SqlDbType.Text).Value = txt_answer.Text;
                addanswer.Parameters.Add("@personid", SqlDbType.Int).Value = currentpersonid;
                addanswer.Parameters.Add("@questid", SqlDbType.Int).Value = currentquestionid;
                addanswer.ExecuteNonQuery();
                questionlistele();
            }
        }
    }
}
