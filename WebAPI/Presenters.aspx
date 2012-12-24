<%@ Page Language="C#" MasterPageFile="~/RightRegister.master" AutoEventWireup="true"
    Inherits="Presenters" Title="For Presenters" Codebehind="Presenters.aspx.cs" %>
    
    
    
<asp:Content ID="SublinksPresenters" ContentPlaceHolderID="Sublinks" runat="server">
    <asp:Menu ID="presentersSubMenu" runat="server" DataSourceID="SiteMapProgram" SkinID="subMenu">
    </asp:Menu>
</asp:Content>



<asp:Content ID="SessionsContent" ContentPlaceHolderID="MainContent" runat="server">



<div class="mainHeading">For Speakers</div>



<%--<div class="pad">

<p>Stay tuned to this page.&nbsp; We plan on having resources and possible get 
    togethers to help veteran and new speakers improve the quality of their 
    presentations.</p>    
    
</div>--%>
      <div style="margin: 20px 20px 20px 20px">
          
<h3>Introduction</h3>
  
        

    
<p>Welcome to Code Camp. By definition code camp is a conference by developers for developers and that means our presenters are drawn from our local development community. This article serves as a guideline to help new presenters put together their first talk.</p>

<h3>&nbsp;</h3>
          <h3>Basics</h3>

<p>Code camp talks are 75 minutes in length. The room provided to you will depend on the number of folks who register for your talk. You may have a small intimate setting for groups under 30 or a larger amphitheater setting for larger groups.</p>

<p>Projectors are provided and usually display best at 1024 x 768. Make sure that your material presents well at that resolution.&nbsp;&nbsp; 
    If for example you are using Visual Studio, change your text to display a 14pt 
    font.&nbsp; If you plan on giving out material, you will need to bring as many 
    copies as you need.&nbsp; We have no access to copy machines at camp.&nbsp; 
    Also, wireless and wired internet will likely be available for speakers.&nbsp; 
    If your session requires the internet, make sure you come come early to verify 
    you have a good connection.&nbsp; </p>
    <p>You will need to bring your own notebook computer to plug in if your 
        session requires that.&nbsp; There may be equipment in the room you are 
        presenting in, but don&#39;t count on anything more than a projector.&nbsp; Same 
        with white or black boards.&nbsp; They may be available, but don&#39;t count on it.</p>
    <p>If you have material you would like to share with your attendees, you can use the 
        wiki link on your session.&nbsp; By default, you wiki points to PBWiki, however 
        if you want to use WetPaint&#39;s integrated wiki, you can change your profile to 
        point to that instead.</p>

<h3>&nbsp;</h3>
          <h3>Choosing a Topic</h3>

<p>There are a few approaches to choosing a topic but for your first talk - choose something you&#8217;re passionate about and/or really interested in. This will make the experience fun and your enthusiasm will carry you through any first time jitters.</p>

<p>You should also select a topic that you can talk about for more than 75 minutes. You don&#8217;t need to know every detail about your topic, but you will be asked questions so you&#8217;ll want to have more depth on the subject than you present.</p>

<h3>&nbsp;</h3>
          <h3>Outline</h3>

<p>Once you&#8217;ve decided on the topic you&#8217;ll need to start planning. Usually you will have a slide deck for some parts of your presentation. At a minimum the slide deck should contain:</p>

<ul>
  <li>A leading slide that contains the presentation title and author information. This should be up and visible as soon as your computer is ready to start so that the audience can quickly determine if they are in the right room. </li>

  <li>An agenda that lays out what you will be covering in the talk and in what order. This prepares the audience for what is to come and helps set their expectations and lays a learning framework. </li>

  <li>A closing slide that contains any information you want to leave them with like your blog or email address. </li>
</ul>

<p>You will have to make some basic assumptions about your audience and some of that can be controlled by identifying the target audience&#8217;s experience level: beginner, intermediate, advanced.</p>

<p>For example, if you are going to give a C# intermediate talk then you can assume that the core of your audience will have written something using C#. These types of assumptions allow you to create a persona of your expected audience attendee. Having a &#8220;person&#8221; to target the talk to will help you plan how you will move through your talk and what you will emphasize. </p>
          <p>&nbsp;</p>

<h3>Flesh Out</h3>

<p>Once you&#8217;ve created the outline, complete the talk by adding depth. This depth may consist of concept slides, code samples, images, tables, sound elements, etc&#8230; all depending on the nature of your topic. Your content should support your talking points so talks that are code specific should have plenty of coding examples while concept talks (like architecture, project management, etc&#8230;) should use images or case studies.</p>

<p>I find it helpful to think of this process as creating a learning journey for my attendees. I have my persona who has some grounding in the technologies I&#8217;m going to cover, and is there to learn about the specific element(s) I&#8217;m presenting on.</p>

<p>During this stage don&#8217;t worry about time. You are simply trying to figure out how much material you must have to support the basic premises of your talk.</p>
          <p>&nbsp;</p>

<h3>Pare Down</h3>

<p>Once you&#8217;ve created all your content - do a timed run through. Do you have too much material? Not enough? <b></b></p>

<p>If you have too much content it is very likely that you have tried to do too much; that your subject is too broad. This is actually a great place to be. Re-evaluate the topic and constrain it. How much to bring it in depends on how much over the one hour time limit you are.</p>

<p>Get the talk down to 60 minutes even though the time limit is 75 because you&#8217;ll want to leave time for questions. Places for questions can be sprinkled through out your talk or just done at the end. How you handle questions is based on how engaged you want to make your audience.</p>

<p>Remember that each attendee is coming to your talk looking for some kernel of wisdom that they can take away. Sometimes it is the question period that will get you the great reviews and this is where having a deep knowledgeable comes in very handy.</p>

<p>If you have to choose between a slide and code&#8230;choose code. I&#8217;ve even seen code put into a slide to zoom in on a particular concept. This technique can actually save you time when discussing small code snippets that don&#8217;t require the development environment.</p>
          <p>&nbsp;</p>

<h3>Rehearse</h3>

<p>Practice alone a few times until you feel comfortable with the material. Then take that big step and ask some close friends to listen to your talk. Ideally you&#8217;ll want some folks who are familiar with the subject and some who are not. Listen to the feedback from both groups. You are likely to hear their questions again and having already answered will make it easier to respond during a &#8220;live&#8221; talk. Additionally, your friends can double check your assumptions, code, and approach. Letting you know what parts look good and what parts need more work.</p>

<p>While rehearsing, make sure that the presentation content, such as code, is clearly visible from the back of an average size conference room.</p>
          <p>&nbsp;</p>

<h3>Present</h3>

<p>Now it&#8217;s time for the presentation.</p>
          <p>&nbsp;</p>
          <p>&nbsp;</p>

<p><b>Before you get there:</b></p>

<ul>
  <li>Get a good night&#8217;s sleep. </li>

  <li>Eat breakfast. </li>

  <li>Wear comfortable clothes. </li>

  <li>Bring water. </li>

  <li>Double check your equipment. Laptop, power cable, mouse, etc&#8230; </li>

  <li>Bring business cards. </li>
    <li> </li>
</ul>

<p><b>Setting up:</b></p>

<p>Be ready to start setting up your machine as soon as the speaker from the last section finishes. </p>

<ul>
  <li>Make sure that the presentation is set up so that the users can see the material. This is especially important for code examples because you&#8217;ll likely have to adjust the font and move some of your tools off the main GUI. </li>

  <li>Pre start as much as you can before the presentation so that you can just switch. This may be a slide deck, development environment, sample files, console window, etc&#8230; </li>

  <li>Make sure that you remembered to reset any code samples to the starting state. It is common to forget to reset these after you&#8217;ve done your practice runs. </li>
</ul>

<p><b>During the presentation:</b></p>

<p><b></b></p>

<ul>
  <li>Breath </li>

  <li>Slow things down; don&#8217;t talk too fast </li>

  <li>Talk to the guy in the back of the room. </li>

  <li>Be yourself.</li>

  <li>Have fun.</li>
</ul>

<p>If you find yourself getting nervous&#8230;randomly pick people in the audience to talk to. Shift your gaze from one person to another after each section.</p>
          <p>&nbsp;</p>

<h3>Summary</h3>

<p>Remember that your audience wants you to succeed. They are there because they believe that you have something to teach them. So have fun and good luck.</p>
</div>


</asp:Content>



