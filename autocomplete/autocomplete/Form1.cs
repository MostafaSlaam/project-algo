using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
namespace autocomplete
{
    public partial class Form1 : Form
    {
       static Trie t = new Trie();
        static List<string> words = new List<string>();
        static List<string> strings = new List<string>();
        public Form1()
        {
            InitializeComponent();
           
        }
        public static int min(int x, int y, int z)
        {
            int min = x;

            if (y < min)
                min = y;

            if (z < min)
                min = z;

            return min;
        }
        static int NumOfSpaces(string s)
        {
            int size = s.Length;
            int x = 0;
            for (int i = 0;i<size ; i++)
            {
                if (s[i] == ' ')
                    x++;
            }
            return x;
        }
        static List<string> Substring(string s)
        {
            List<string> l = new List<string>();
             string temp;
            int size=strings.Count;
            
            for (int k = 0; k < size;k++ )
            {
                temp = strings[k];

                int l1 = s.Length;
                int l2 = strings[k].Length;
                bool flag = false;
                if (l1 > l2)
                {
                    flag = false;
                    break;
                }
                for (int i = 0; i < l2; i++)
                {
                    if (temp[i] == s[0])
                    {
                        for (int j = 1; j < l1; j++)
                        {

                            if (temp[i + j] == s[j])
                            {
                                if (j == l1 - 1)
                                    flag = true;
                            }
                            else
                                break;
                        }
                    }
                }
                if (flag == true)
                {
                    l.Add(temp);
                }
            }

            return l;
        }
        //static bool IsSubsequence2(string s, string t)
        //{
        //    //To get the length of the strings: s.Length or t.Length
        //    bool flag = false;
        //    int ss = s.Length;
        //    int sss = s.Length;
        //    int x = 0;
        //    int ts = t.Length;
        //    if (ts < ss)
        //    {
        //        return false;
        //    }
        //    for (int i = 0; i < ts; i++)
        //    {

        //        if (s[x] == t[i])
        //        {
        //            ss--;
        //            x++;
        //        }

        //        if (ss == 0)
        //        {
        //            flag = true;
        //            break;
        //        }
        //    }
        //    return flag;
        //}
        //static List<string> near(string s)
        //{
        //    List<string>l=new List<string>();
        //    string x;
        //    int size = words.Count ;
        //    for (int i = 0; i < size; i++)
        //    {
        //        if (words[i].Length > s.Length)
        //        {
        //            x = words[i];
        //            if (IsSubsequence2(s,x))
        //            {
        //                l.Add(words[i]);
        //            }
        //        }
        //        else
        //        {
        //            if (IsSubsequence2( words[i],s))
        //            {
        //                l.Add(words[i]);
        //            }
        //        }

        //    }
        //    return l;
        //}
        public static int CalculateMinNumberOfWeightedChanges(string s1, string s2 )
        {
            int s1length=s1.Length;
            int s2length=s2.Length;

            int[,] arr = new int[s1length + 1, s2length + 1];
            int deleteWeight=1;
            int replaceWeight=1;
            int insertWeight=1;



            for (int i = 0; i < s1length + 1; i++)
            {
                for (int j = 0; j < s2length + 1; j++)
                {
                    if (i == 0 && j == 0)
                    {
                        arr[0, 0] = 0;
                    }
                    else if (i == 0)
                    {
                        arr[0, j] = arr[0, j - 1] + insertWeight;
                    }
                    else if (j == 0)
                    {
                        arr[i, 0] = arr[i - 1, 0] + deleteWeight;
                    }
                    else if (s1[i - 1] == s2[j - 1])
                    {
                        arr[i, j] = arr[i - 1, j - 1];
                    }
                    else
                    {
                        int r1 = arr[i - 1, j] + deleteWeight;
                        int r2 = arr[i - 1, j - 1] + replaceWeight;
                        int r3 = arr[i, j - 1] + insertWeight;

                        arr[i, j] = min(r1, r2, r3);
                    }
                }

            }
            return arr[s1length, s2length];
           
        }
        static string edit_word(string s)
        {
            List<string> mylist = new List<string>();
            int mini=100;
            string str=" ";
            string temp=" ";
                int size = words.Count;
                for (int i = 0; i < size; i++)
                {
                    if (mini > CalculateMinNumberOfWeightedChanges(s, words[i]))
                    {
                        temp = str;
                        mini = CalculateMinNumberOfWeightedChanges(s, words[i]);
                        str = words[i];
                    }
                
            }
            return str;
        }
        static List<string> fromtrie(string s)
        {
            List<string> l = new List<string>();
            List<line> nl = new List<line>();
            string temp;
            //list from trie with result of search
            l = t.GetCompletionList(s);
            List<string> result = new List<string>();
            int size = l.Count();
            line[] arr = new line[size];
            line[] narry = new line[size];
            //fill the result into an arry
            for (int i = 0; i < size; i++)
            {
                temp = l[i];
                string[] fields = temp.Split(',');
                line x = new line(int.Parse(fields[1]), fields[0]);
                arr[i] = x;

            }
            narry = MergeSort(arr);
            for (int i = 0; i < size; i++)
            {
                result.Add(narry[i].s);
            }
            return result;
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
      {
            try
            {
                List<string>l=new List<string>();
                if (textBox1.Text.Length > 0)
                {
                    l = fromtrie(textBox1.Text);
                    listBox1.ResetText();
                    listBox1.DataSource = l;
                    if (l.Count == 0)
                    {
                        l =Substring(textBox1.Text.ToLower());
                        listBox1.ResetText();
                        listBox1.DataSource = l;
                   
                    }

                    if (l.Count == 0)  
                        {
                            List<string> result = new List<string>();
                        int size = NumOfSpaces(textBox1.Text);
                        if(size!=0)
                        {
                            string[] fields = textBox1.Text.Split(' ');
                            for (int i = 0; i < size+1;i++ )
                            {
                                if (!check(fields[i]))
                                {
                                    fields[i] = edit_word(fields[i]);
                                }
                            }
                            string tem=fields[0];
                            for (int i = 1; i < size + 1; i++)
                            {
                                tem = tem + fields[i] + " ";
                            }
                            result=Substring(tem);
                            if (result.Count == 0)
                            {

                               // var x = new List<string>();
                                List<string> temp = new List<string>();
                                for (int i = 0; i < size; i++)
                                {

                                    temp = Substring(fields[i]);
                                    result.AddRange(temp);

                                }
                            }
                            listBox1.DataSource = result;
                        }
                        else
                        {
                            string s = edit_word(textBox1.Text);
                            l = Substring(s);   
                            listBox1.ResetText();
                            listBox1.DataSource = l;

                        }
                    }
                 
                 listBox1.ResetText();

                }
                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            FileStream fs = new FileStream("Search Links (Scenario 1 & 2).txt", FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            string temp;
            while (sr.Peek() != -1)
            {
                temp = sr.ReadLine();
                string[] fields = temp.Split(',');
                t.Add(fields[1] + "," + fields[0]);
                strings.Add(fields[1].ToLower());
            }

            fs.Close();


            FileStream fss = new FileStream("Sample Dictionary.txt", FileMode.Open);
            StreamReader ssr = new StreamReader(fss);
            string temp2;
            while (ssr.Peek() != -1)
            {
                temp2 = ssr.ReadLine();
                words.Add(temp2.ToLower());
            }

            fss.Close();
        }



        private void button1_Click(object sender, EventArgs e)
        {
            List<string> l = new List<string>();
            List<line> nl = new List<line>();
            string temp;
            l = t.GetCompletionList(textBox1.Text);
            int size = l.Count();
            line [] arr=new line[size];
            line[] narry = new line[size];
            for (int i = 0; i < size;i++ )
            {
                temp = l[i];
                string[] fields = temp.Split(',');
                line x = new line(int.Parse(fields[1]),fields[0]);
                arr[i] = x;
                
            }

            narry = MergeSort(arr);
            dataGridView1.DataSource = narry;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            List<string> l = new List<string>();
            List<line> nl = new List<line>();
            string temp;
            l = t.GetCompletionList(textBox1.Text);
            int size = l.Count();
            line[] arr = new line[size];
            line[] narry = new line[size];
            for (int i = 0; i < size; i++)
            {
                temp = l[i];
                string[] fields = temp.Split(',');
                line x = new line(int.Parse(fields[1]), fields[0]);
                arr[i] = x;

            }
            narry = performInsertionSort(arr);
            dataGridView2.DataSource = narry;


        }
       
        public static line[] MergeSort(line[] array)
        {
            // If list size is 0 (empty) or 1, consider it sorted and return it
            // (using less than or equal prevents infinite recursion for a zero length array).
            if (array.Length <= 1)
            {
                return array;
            }

            // Else list size is > 1, so split the list into two sublists.
            int middleIndex = (array.Length) / 2;
            line[] left = new line[middleIndex];
            line[] right = new line[array.Length - middleIndex];

            Array.Copy(array, left, middleIndex);
            Array.Copy(array, middleIndex, right, 0, right.Length);

            // Recursively call MergeSort() to further split each sublist
            // until sublist size is 1.
            left = MergeSort(left);
            right = MergeSort(right);

            // Merge the sublists returned from prior calls to MergeSort()
            // and return the resulting merged sublist.
            return Merge(left, right);
        }

        public static line[] Merge(line[] left, line[] right)
        {
            // Convert the input arrays to lists, which gives more flexibility 
            // and the option to resize the arrays dynamically.
            List<line> leftList = left.OfType<line>().ToList();
            List<line> rightList = right.OfType<line>().ToList();
            List<line> resultList = new List<line>();

            // While the sublist are not empty merge them repeatedly to produce new sublists 
            // until there is only 1 sublist remaining. This will be the sorted list.
            while (leftList.Count > 0 || rightList.Count > 0)
            {
                if (leftList.Count > 0 && rightList.Count > 0)
                {
                    // Compare the 2 lists, append the smaller element to the result list
                    // and remove it from the original list.
                    if (leftList[0].w<= rightList[0].w)
                    {
                        resultList.Add(leftList[0]);
                        leftList.RemoveAt(0);
                    }

                    else
                    {
                        resultList.Add(rightList[0]);
                        rightList.RemoveAt(0);
                    }
                }

                else if (leftList.Count > 0)
                {
                    resultList.Add(leftList[0]);
                    leftList.RemoveAt(0);
                }

                else if (rightList.Count > 0)
                {
                    resultList.Add(rightList[0]);
                    rightList.RemoveAt(0);
                }
            }

            // Convert the resulting list back to a static array
            line[] result = resultList.ToArray();
            return result;
        }

        public static line[] performInsertionSort(line[] inputarray)
        {
            for (int i = 0; i < inputarray.Length - 1; i++)
            {
                int j = i + 1;

                while (j > 0)
                {
                    if (inputarray[j - 1].w > inputarray[j].w)
                    {
                        line temp = inputarray[j - 1];
                        inputarray[j - 1] = inputarray[j];
                        inputarray[j]= temp;

                    }
                    j--;
                }
            }
            return inputarray;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            string w;


             w = (textBox1.Text);
            
            string[] sss = w.Split(' ');
            List<string> wrong = new List<string>();
            int count = 0;
            List<int> ar = new List<int>();
            for (int i = 0; i < sss.Length; i++)
            {   if (sss[i] == "")
                { i++;
               
                continue;
                }
                bool f = check(sss[i]);
                if (!f)
                {
                    count = 1;

                    ar.Add(i);
                    wrong.Add(sss[i]);
                }

            }

            if (count == 1)
            {
                MessageBox.Show("your sentence false");
               
            }
            else
               
                MessageBox.Show("your sentence true");
        }

        public static bool check(string w)
        {
            
            FileStream fs = new FileStream("dictionary.txt", FileMode.Open);
            StreamReader fr = new StreamReader(fs);
            List<string> l = new List<string>();

            while (fr.Peek() != -1)
            {
                string st = fr.ReadLine();
                l.Add(st);
            }
            fr.Close();
            
            int mid = (l.Count) / 2;
            string com = l[mid];
            if (w.Length > 0)
            {
                if (com[0] > w[0])
                {
                    for (int i = 1; i <= mid; i++)
                    {
                        if (w == l[i])
                        {
                            return true;
                        }
                    }
                }
                if (com[0] < w[0])
                {
                    for (int i = mid + 1; i < l.Count; i++)
                    {
                        if (w == l[i])
                            return true;
                    }

                }
            }
            return false;
        }

       
    }
    }
    

