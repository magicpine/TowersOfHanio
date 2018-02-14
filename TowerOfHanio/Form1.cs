using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace TowerOfHanio
{
    /// <summary>
    /// The form for the game
    /// </summary>
    public partial class Form1 : Form
    {
        #region Variables

        private List<PostData> _posts;
        private int _totalMoves;

        #endregion Varaibles

        #region Constructor

        /// <summary>
        /// Constructor for the form
        /// </summary>
        public Form1()
        {
            InitializeComponent();
        }

        #endregion Constructor

        #region Events

        /// <summary>
        /// Handles the Start Game button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            //Clear the Old Game out
            lsLog.Items.Clear();
            _totalMoves = 0;
            //Add the Post Data in the List
            _posts = new List<PostData>();
            _posts.Add(new PostData(Posts.First));
            _posts.Add(new PostData(Posts.Second));
            _posts.Add(new PostData(Posts.Third));
            //Put all the Disks that are part of the game into the First Post 
            List<int> tmp = new List<int>();
            for (int i = 0; i < numericUpDown1.Value; i++)
                tmp.Add(i);
            _posts[(int)Posts.First].disksOnPost = tmp;
            //Show the very first one 
            LogMove(false);
            //Now Solve The Game
            MoveAllDisks(numericUpDown1.Value, _posts[(int)Posts.First], _posts[(int)Posts.Third]);
        }

        /// <summary>
        /// moves all disks from one pole to another
        /// </summary>
        /// <param name="value"></param>
        /// <param name="OG"></param>
        /// <param name="Dest"></param>
        private void MoveAllDisks(decimal value, PostData OG, PostData Dest)
        {
            //If Theres one disk left then just move it to the dest post
            if (value == 1)
            {
                MoveSingleDisk(OG, Dest);
                return;
            }
            //Find the Post that isn't chosen.
            Posts tmp = FindPostThatWasntChosen(OG.Name, Dest.Name);
            if (tmp == Posts.NoPost)
                return;
            PostData postThatWasntChosen = _posts.Find(x => x.Name == tmp);
            //Move Disks from the OG post to the Other Post
            MoveAllDisks(value - 1, OG, postThatWasntChosen);
            //Move the Last Disk from the OG to the Dest pole
            MoveSingleDisk(OG, Dest);
            //Now Move the Disks from the PostThatWasntChosen to the Dest
            MoveAllDisks(value - 1, postThatWasntChosen, Dest);
        }

        #endregion Events

        #region Methods

        /// <summary>
        /// Moves a single disk 
        /// </summary>
        /// <param name="OG"></param>
        /// <param name="Dest"></param>
        private void MoveSingleDisk(PostData OG, PostData Dest)
        {
            //The smallest number in the list will be the only disk you can move
            Dest.disksOnPost.Add(OG.disksOnPost.Min());
            OG.disksOnPost.Remove(OG.disksOnPost.Min());
            LogMove(true);
        }

        /// <summary>
        /// Logs the Post Data to the ListBox
        /// </summary>
        /// <param name="updateMoves"></param>
        private void LogMove(bool updateMoves)
        {
            if(updateMoves)
                _totalMoves++;
            lsLog.Items.Add("---------------------------------------------------------Total Moves = " + _totalMoves.ToString());
            lsLog.Items.Add("Post One has " + _posts[(int)Posts.First].disksOnPost.Count + " disks on it");
            lsLog.Items.Add("Post Two has " + _posts[(int)Posts.Second].disksOnPost.Count + " disks on it");
            lsLog.Items.Add("Post Third has " + _posts[(int)Posts.Third].disksOnPost.Count + " disks on it");
            lsLog.SelectedIndex = lsLog.Items.Count-1;
            Application.DoEvents();
            Thread.Sleep(900);
        }

        /// <summary>
        /// Finds the Post that Wasn't chosen
        /// </summary>
        /// <param name="first"></param>
        /// <param name="third"></param>
        /// <returns></returns>
        private static Posts FindPostThatWasntChosen(Posts first, Posts third)
        {
            switch (first)
            {
                case Posts.First:
                    switch (third)
                    {
                        case Posts.Second:
                            return Posts.Third;
                        case Posts.Third:
                            return Posts.Second;
                    }
                    break;
                case Posts.Second:
                    switch (third)
                    {
                        case Posts.First:
                            return Posts.Third;
                        case Posts.Third:
                            return Posts.First;
                    }
                    break;
                case Posts.Third:
                    switch (third)
                    {
                        case Posts.Second:
                            return Posts.First;
                        case Posts.First:
                            return Posts.Second;
                    }
                    break;
            }
            return Posts.NoPost;
        }

        #endregion Methods
    }

    /// <summary>
    /// Contains the data for each Post
    /// </summary>
    public class PostData
    {
        #region Properties

        /// <summary>
        /// The name of the post
        /// </summary>
        public Posts Name;
        /// <summary>
        /// The disks that exist on the post
        /// </summary>
        public List<int> disksOnPost { get; set; }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// The Constructor
        /// </summary>
        /// <param name="name">Name of the post</param>
        public PostData(Posts name)
        {
            Name = name;
            disksOnPost = new List<int>();
        }

        #endregion Constructor
    }

    /// <summary>
    /// The Names for the Post
    /// </summary>
    public enum Posts
    {
        First,
        Second,
        Third,
        NoPost,
    }
}
