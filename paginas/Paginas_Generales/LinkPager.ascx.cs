using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class LinkPager : System.Web.UI.UserControl
{
    public event CustomDelegateClass.PageChangedEventHandler PageChanged;

    private int _currentPageNumber;
    public int CurrentPageNumber
    {
        get { return _currentPageNumber; }
        set { _currentPageNumber = value; }
    }

    private int _totalPages;
    public int TotalPages
    {
        get { return _totalPages; }
        set { _totalPages = value; }
    }

    System.Collections.Generic.List<int> lstPageNumbers;
    CustomPageChangeArgs args;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            this.CurrentPageNumber = 1;
            BindRepeater();

            lblCurrentPage.Text = this.CurrentPageNumber.ToString();
            lblTotalRecords.Text = this.TotalPages.ToString();

            SetUnsetLinkButtons();
        }
    }

    protected void rptPages_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            if (Convert.ToInt32(e.Item.DataItem) == this.CurrentPageNumber)
            {
                ((LinkButton)e.Item.FindControl("lnkPageNumbers")).Style[HtmlTextWriterStyle.Display] = "none";
                ((Label)e.Item.FindControl("lblPageNumbers")).Text = e.Item.DataItem.ToString();
            }
            else
            {
                ((LinkButton)e.Item.FindControl("lnkPageNumbers")).Text = e.Item.DataItem.ToString();
                ((Label)e.Item.FindControl("lblPageNumbers")).Style[HtmlTextWriterStyle.Display] = "none";
            }
        }
    }

    protected void lnkPageNumbers_Click(object sender, EventArgs e)
    {
        args = new CustomPageChangeArgs();
        args.CurrentPageNumber = Convert.ToInt32(((LinkButton)sender).Text);
        this.CurrentPageNumber = args.CurrentPageNumber;
        Pager_PageChanged(this, args);

        BindRepeater();
        lblCurrentPage.Text = this.CurrentPageNumber.ToString();
        SetUnsetLinkButtons();
    }

    protected void lnkGOFPage_Click(object sender, EventArgs e)
    {
        args = new CustomPageChangeArgs();
        switch (((LinkButton)sender).ID)
        {
            case "lnkFirstPage": args.CurrentPageNumber = 1; HttpContext.Current.Items.Add("currentPage", 1);
                break;
            case "lnkPreviousPage": args.CurrentPageNumber = Convert.ToInt32(lblCurrentPage.Text) - 1;
                break;
            case "lnkNextPage": args.CurrentPageNumber = Convert.ToInt32(lblCurrentPage.Text) + 1;
                break;
            case "lnkLastPage": args.CurrentPageNumber = Convert.ToInt32(lblTotalRecords.Text);
                break;
        }
        
        this.CurrentPageNumber = args.CurrentPageNumber;
        Pager_PageChanged(this, args);

        BindRepeater();
        lblCurrentPage.Text = this.CurrentPageNumber.ToString();
        SetUnsetLinkButtons();
    }

    void Pager_PageChanged(object sender, CustomPageChangeArgs e)
    {
        PageChanged(this, e);
        //throw new Exception("The method or operation is not implemented.");
    }

    private void BindRepeater()
    {
        lstPageNumbers = new System.Collections.Generic.List<int>();
        for (int count = 1; count <= this.TotalPages; ++count)
        {
            lstPageNumbers.Add(count);
        }

        rptPages.DataSource = lstPageNumbers;
        rptPages.DataBind();
    }

    private void SetUnsetLinkButtons()
    {
        if (this.CurrentPageNumber <= 1)
        {
            lnkFirstPage.Style[HtmlTextWriterStyle.Display] = "none";
            lnkPreviousPage.Style[HtmlTextWriterStyle.Display] = "none";
            lblFirstPage.Style[HtmlTextWriterStyle.Display] = "inline";
            lblPreviousPage.Style[HtmlTextWriterStyle.Display] = "inline";
            lblLastPage.Style[HtmlTextWriterStyle.Display] = "none";
            lblNextPage.Style[HtmlTextWriterStyle.Display] = "none";
            lnkLastPage.Style[HtmlTextWriterStyle.Display] = "inline";
            lnkNextPage.Style[HtmlTextWriterStyle.Display] = "inline";
        }
        else if (this.CurrentPageNumber >= this.TotalPages)
        {
            lnkFirstPage.Style[HtmlTextWriterStyle.Display] = "inline";
            lnkPreviousPage.Style[HtmlTextWriterStyle.Display] = "inline";
            lblFirstPage.Style[HtmlTextWriterStyle.Display] = "none";
            lblPreviousPage.Style[HtmlTextWriterStyle.Display] = "none";
            lblLastPage.Style[HtmlTextWriterStyle.Display] = "inline";
            lblNextPage.Style[HtmlTextWriterStyle.Display] = "inline";
            lnkLastPage.Style[HtmlTextWriterStyle.Display] = "none";
            lnkNextPage.Style[HtmlTextWriterStyle.Display] = "none";
        }
        else
        {
            lnkFirstPage.Style[HtmlTextWriterStyle.Display] = "inline";
            lnkPreviousPage.Style[HtmlTextWriterStyle.Display] = "inline";
            lblFirstPage.Style[HtmlTextWriterStyle.Display] = "none";
            lblPreviousPage.Style[HtmlTextWriterStyle.Display] = "none";
            lblLastPage.Style[HtmlTextWriterStyle.Display] = "none";
            lblNextPage.Style[HtmlTextWriterStyle.Display] = "none";
            lnkLastPage.Style[HtmlTextWriterStyle.Display] = "inline";
            lnkNextPage.Style[HtmlTextWriterStyle.Display] = "inline";
        }
    }
}
