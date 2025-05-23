using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace refund
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            cmbServiceType.Items.Add("12 = سالانه");
            cmbServiceType.Items.Add("1 = ماهانه");
            cmbServiceType.Items.Add("3 = سه‌ماهه");
            cmbServiceType.Items.Add("6 = شش‌ماهه");
            cmbServiceType.SelectedIndex = 0;

            dtpStartDate.Value = DateTime.Now.AddMonths(-1); 
            dtpEndDate.Value = DateTime.Now;
        }

       

        private void Form1_Load(object sender, EventArgs e)
        {
           // DateTime today = DateTime.Now;

           // cmbTyearF.SelectedItem = today.Year.ToString();
           // cmbTmonthF.SelectedItem = today.Month.ToString("D2");
          //  cmbTdayF.SelectedItem = today.Day.ToString("D2");
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            try
            {
                double X = Convert.ToDouble(txtX.Text);

                // گرفتن نوع سرویس از کمبو باکس
                int serviceType = Convert.ToInt32(cmbServiceType.SelectedItem.ToString().Split('=')[0].Trim());

                // دریافت تاریخ شروع و پایان
                DateTime startDate = dtpStartDate.Value;
                DateTime finishDate = dtpEndDate.Value;

                // تعداد روزهای استفاده‌شده
                TimeSpan differenceUsed = finishDate - startDate;
                int T_used = (int)differenceUsed.TotalDays;

                // تعداد روزهای کل برای هر نوع سرویس
                int T_total;
                switch (serviceType)
                {
                    case 12:  // سالانه
                        T_total = 365;
                        break;
                    case 1:  // ماهانه
                        T_total = 30;
                        break;
                    case 3:  // سه‌ماهه
                        T_total = 90;
                        break;
                    case 6:  // شش‌ماهه
                        T_total = 183;
                        break;
                    default:
                        MessageBox.Show("نوع سرویس نامعتبر است.");
                        return;
                }

                // تعداد روزهای باقی‌مانده
                int T_remaining = T_total - T_used;
                if (T_remaining < 0)
                {
                    T_remaining = 0;
                }

                // نمایش روزهای باقی‌مانده
                txtuse.Text = $"روز {T_remaining} ";

                // محاسبه میزان بازپرداخت
                double refund = CalculateRefund(X, T_used, serviceType, T_remaining);
                txtRefund.Text = refund.ToString("N0");
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطا در محاسبه: " + ex.Message);
            }
        }

        private double CalculateRefund(double X, int T_used, int serviceType, int T_remaining)
        {
            double X_after_discount = X / 1.1;
            double Y_after_service_discount = X_after_discount * 0.8;

            int T_total = 0;

            switch (serviceType)
            {
                case 12: // سالانه
                    T_total = 365;
                    break;
                case 1: // ماهانه
                    T_total = 30;
                    break;
                case 3: // سه‌ماهه
                    T_total = 90;
                    break;
                case 6: // شش‌ماهه
                    T_total = 183;
                    break;
                default:
                    MessageBox.Show("نوع سرویس نامعتبر است.");
                    return 0;
            }

            double refund = (Y_after_service_discount / T_total) * T_remaining;
            return refund;
        }

            private void btnclear_Click(object sender, EventArgs e)
        {
            txtX.Clear(); 
            txtuse.Clear(); 
            txtRefund.Clear(); 

            cmbServiceType.SelectedIndex = 0; 

            dtpStartDate.Value = DateTime.Now.AddMonths(-1); 
            dtpEndDate.Value = DateTime.Now;
        }
    }
}
