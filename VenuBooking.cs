using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Venue_Booking
{
    public partial class VenuBooking : Form
    {
        string[,] reservation = new string[5, 6];
        string[] waiting = new string[10];

        public VenuBooking()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();     //close the window 
        }

        private void btnBook_Click(object sender, EventArgs e)     //buttton for booking
        {
            lblMessage.Text = "";    //label for message
            txtReservation.Text = "";   //rich text box for reservation
            bool seatFilled = true;     //boolen for seat is filled or not 

            for (int i = 0; i < reservation.GetLength(0); i++)   // for loop  for row
            {
                string row = lstRow.Items[i].ToString();    //store the value of list box (like A,B,C,D,E)
                for (int j = 0; j < reservation.GetLength(1); j++)   // for loop for seats
                {
                    string seat = lstSeat.Items[j].ToString();   // store the value of seats

                    if (string.IsNullOrEmpty(reservation[i, j]))  //cheak if its empty ot=r not
                    {
                        if (lstRow.SelectedIndex == -1 || lstSeat.SelectedIndex == -1 || string.IsNullOrEmpty(txtName.Text)) //check if user selected row and colum
                        {
                            if (lstRow.SelectedIndex == -1)
                            {
                                lblMessage.Text = "Please select the row" + "\n";  //message
                            }
                            if (lstSeat.SelectedIndex == -1)
                            {
                                lblMessage.Text += "Please select seats" + "\n";
                            }
                            if (string.IsNullOrEmpty(txtName.Text))
                            {
                                lblMessage.Text += "name required" + "\n";
                            }
                            return;
                        }

                        if (lstRow.SelectedItem.ToString() == row && lstSeat.SelectedItem.ToString() == seat)  //find the selected row and seats 
                        {
                            reservation[i, j] = txtName.Text;    // take the value from textbox 
                            lblMessage.Text = "Booked sucessfully";//messsage
                            seatFilled = false; 
                        }
                    }
                    else if (!string.IsNullOrEmpty(reservation[i, j]) && seatFilled)   //check if string is not empty and some is already there 
                    {
                        lblMessage.Text = "Seat is already booked";  //message
                        btnReservation_Click(sender, e);   //call method
                    }
                }
            }
            //clear row, seat, text
            txtName.Text = "";  
            lstRow.ClearSelected();
            lstSeat.ClearSelected();
              btnReservation_Click(sender,e);

        }

        private void btnReservation_Click(object sender, EventArgs e)   // the reservatipon list
        {

            txtReservation.Text = ""; 


            for (int i = 0; i < reservation.GetLength(0); i++)  
            {
                string row = lstRow.Items[i].ToString();
                for (int j = 0; j < reservation.GetLength(1); j++)
                {
                    string seat = lstSeat.Items[j].ToString();  
                    txtReservation.Text += row + seat + " - " + reservation[i, j] + "\n"; //print all reservations in richtextbox
                }

            }

        }
        private void btnWait_Click(object sender, EventArgs e)   //button for add to waiting list
        {
            lblMessage.Text = "";
            bool seatAvailable = false;  //boolen for seat available
            bool seatFilled = false;    //boolen for seat is filled 
            bool waitListIsFull = false;   //boolen for wait list is full 
            for (int i = 0; i < reservation.GetLength(0); i++)
            {
                string row = lstRow.Items[i].ToString();
                for (int j = 0; j < reservation.GetLength(1); j++)
                {
                    string seat = lstSeat.Items[j].ToString();
                    if (string.IsNullOrEmpty(reservation[i, j]))   // check empty string 
                    {
                        seatAvailable = true;   //true for seat availalbel 
                    }
                    else if (!string.IsNullOrEmpty(reservation[i, j]) && (!seatAvailable && !seatFilled)) //seat not available
                    {
                        for (int a = 0; a < waiting.Length; a++)
                        {
                            if (string.IsNullOrEmpty(waiting[a])) //check empty string in waiting part
                            {
                                waiting[a] = txtName.Text;  //take data from text box and store in to waiting list
                                seatFilled = true;  
                                lblMessage.Text = "added to waiting list"; //message
                                break;  
                            }
                            else if (a == waiting.Length - 1 && !string.IsNullOrEmpty(waiting[a]))  //wait list will only store 10 values 
                            {
                                waitListIsFull = true;   //boolen true for wait list full
                                btnWaitlist_Click(sender, e);  
                            }

                        }

                    }

                }
               
            }

            if (seatAvailable)   //message
            {
                lblMessage.Text += "There is seat available";
                btnReservation_Click(sender, e);
            }
            if (waitListIsFull) //message
            {
                lblMessage.Text += "Waiting list is full";
            }
            txtName.Text = "";

        }

        private void btnCancel_Click(object sender, EventArgs e) //cancle button 
        {
            lblMessage.Text = ""; 
            string title = "Canceling booking"; // title for message box
            bool changeCustomer = false;  
            MessageBoxButtons buttons = MessageBoxButtons.YesNo; //message yes and no

            for (int i = 0; i < reservation.GetLength(0); i++)
            {
                string row = lstRow.Items[i].ToString();
                for (int j = 0; j < reservation.GetLength(1); j++)
                {
                    string seat = lstSeat.Items[j].ToString();
                    
                    if (!string.IsNullOrEmpty(reservation[i, j]))   //check no empty string
                    {
                        if (lstRow.SelectedIndex == -1 || lstSeat.SelectedIndex == -1)//check if row and colum is selected
                        {
                            if (lstRow.SelectedIndex == -1)
                            {
                                lblMessage.Text += "Please select the row" + "\n";

                            }
                            if (lstSeat.SelectedIndex == -1)
                            {
                                lblMessage.Text += "Please select seats" + "\n";
                            }
                            return;
                        }
                        else if (lstRow.SelectedItem.ToString() == row && lstSeat.SelectedItem.ToString() == seat) //find row and seat form list boxes
                        {
                            DialogResult result = MessageBox.Show(reservation[i, j] + ": Are you sure you want to cancel the booking?", title, buttons); //for confirmation
                            if (result == DialogResult.Yes) //if user click yes 
                            {
                                reservation[i, j] = null;  //null will stored at that place 
                                lblMessage.Text += "Sucessfull cancelling seats" + "\n"; 

                                reservation[i, j] = waiting[0];  //first person from waiting list will go to reservation part
                                for (int a = 1; a < waiting.Length; a++)
                                {
                                    if (!string.IsNullOrEmpty(waiting[a - 1]))  //check for empty
                                    {
                                        waiting[a - 1] = waiting[a]; //replace array so array 2 will go to 1
                                        changeCustomer = true;  //customer changed
                                        btnWaitlist_Click(sender, e); 
                                    }

                                }

                            }
                            else
                            { //clear row and seats list
                                lstRow.ClearSelected();
                                lstSeat.ClearSelected();
                                return;
                            }
                        }
                    }
                }
            }
            if (changeCustomer)
            {
                lblMessage.Text += "Customer booked to cancel seat" + "\n";  //message 
            }

            btnReservation_Click(sender, e);
            txtName.Text = "";
            lstRow.ClearSelected();
            lstSeat.ClearSelected();
        }

        private void btnFillAllSeats_Click(object sender, EventArgs e)   //fill all the seats with same name
        {
            lblMessage.Text = "";
            for (int i = 0; i < reservation.GetLength(0); i++) 
            {
                string row = lstRow.Items[i].ToString();
                for (int j = 0; j < reservation.GetLength(1); j++)
                {
                    string seat = lstSeat.Items[j].ToString();

                    if (string.IsNullOrEmpty(txtName.Text)) //ask for name if not entered
                    {
                        lblMessage.Text += "name required" + "\n";  
                        return;
                    }
                    reservation[i, j] = txtName.Text; //text vales stored to arrays

                }

                lblMessage.Text = "Add all seats sucessfully";
                btnReservation_Click(sender, e);
            }
            txtName.Text = "";
               lstRow.ClearSelected();
            lstSeat.ClearSelected();
        }

        private void btnWaitlist_Click(object sender, EventArgs e) //waiting list button
        {
            txtWaiting.Clear();   // clear the text box 
            for (int a = 0; a < waiting.Length; a++)  
            {
                txtWaiting.Text += waiting[a] + "\n";  // print value of waiting 
            }
            txtReservation.Text = "";
        }
    }
}
