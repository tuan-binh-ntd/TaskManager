namespace TaskManager.Application.Core.Emails;

public static class EmailContentConstants
{
    public static string CreatedIssueContent(CreatedIssueEmailContentDto createdIssueEmailTemplateDto) => $@"
        <tbody style=""border-bottom:0"">
            <tr>
                <td rowspan=""6""
                    style=""padding:0;padding:0;padding-top:14px;vertical-align:top;width:40px""
                    width=""40""><img
                        src=""{createdIssueEmailTemplateDto.AvatarUrl}""
                        style=""border:0;border:0;border-radius:50.0%;width:32px;height:32px""
                        width=""32"" height=""32"" class=""CToWUd"" data-bit=""iit""></td>
                <td colspan=""2""
                    style=""padding:0;padding:0;padding-top:14px;padding-bottom:6px;line-height:20px;vertical-align:middle"">
                    <span
                        style=""color:#42526e;font-weight:500;display:inline-block;vertical-align:middle"">{createdIssueEmailTemplateDto.ReporterName}</span>
                        <span style=""color:#42526e;display:inline-block;vertical-align:middle"">{createdIssueEmailTemplateDto.IssueCreationTime:hh:mm tt}</span></td>
            </tr>
            <tr>
                <td 
                    style=""padding:0;padding:0;white-space:nowrap;width:1.0%;vertical-align:top;padding-right:5px;line-height:24px"">Issue Type:</td>
                <td 
                    style=""padding:0;padding:0;line-height:24px"">
                    <span 
                        style=""max-width:700px;height:auto;display:inline-block;padding:0 4px"">{createdIssueEmailTemplateDto.IssueTypeName}</span>
                </td>
            </tr>
            <tr>
                <td
                    style=""padding:0;padding:0;white-space:nowrap;width:1.0%;vertical-align:top;padding-right:5px;line-height:24px"">
                    Assignee:</td>
                <td
                    style=""border:0;padding:0;padding:0;padding:0;line-height:24px;display:inline-block;padding:0 4px;display:inline"">
                    <img src=""{createdIssueEmailTemplateDto.AssigneeAvatarUrl}""
                        style=""border:0;height:24px;width:24px;max-width:24px;border:0;max-width:700px;height:auto;border-radius:50.0%;vertical-align:middle;display:inline""
                        height=""24"" width=""24"" class=""CToWUd"" data-bit=""iit"" ></td>
                <td
                    style=""border:0;padding:0;padding:0;padding:0;line-height:24px;display:inline-block;padding:0 4px;display:inline"">
                    <span
                        style=""max-width:700px;height:auto;display:inline-block;padding:0 4px;display:inline"">{createdIssueEmailTemplateDto.AssigneeName}</span></td>
            </tr>
            <tr>
                <td
                    style=""padding:0;padding:0;white-space:nowrap;width:1.0%;vertical-align:top;padding-right:5px;line-height:24px"">
                    Priority:</td>
                <td style=""padding:0;padding:0;line-height:24px""><span
                        style=""max-width:700px;height:auto;display:inline-block;padding:0 4px"">{createdIssueEmailTemplateDto.PriorityName}</span>
                </td>
            </tr>
            <tr>
                <td
                    style=""padding:0;padding:0;white-space:nowrap;width:1.0%;vertical-align:top;padding-right:5px;line-height:24px"">
                    Created:</td>
                <td style=""padding:0;padding:0;line-height:24px""><span
                        style=""max-width:700px;height:auto;display:inline-block;padding:0 4px"">{createdIssueEmailTemplateDto.IssueCreationTime:dd/MMM/yy hh:mm tt}</span></td>
            </tr>
            <tr>
                <td
                    style=""padding:0;padding:0;white-space:nowrap;width:1.0%;vertical-align:top;padding-right:5px;line-height:24px"">
                    Reporter:</td>
                <td
                    style=""border:0;padding:0;padding:0;padding:0;line-height:24px;display:inline-block;padding:0 4px;display:inline"">
                    <img src=""{createdIssueEmailTemplateDto.AvatarUrl}""
                        style=""border:0;height:24px;width:24px;max-width:24px;border:0;max-width:700px;height:auto;border-radius:50.0%;vertical-align:middle;display:inline""
                        height=""24"" width=""24"" class=""CToWUd"" data-bit=""iit""></td>
                <td
                    style=""border:0;padding:0;padding:0;padding:0;line-height:24px;display:inline-block;padding:0 4px;display:inline"">
                    <span
                        style=""max-width:700px;height:auto;display:inline-block;padding:0 4px;display:inline"">{createdIssueEmailTemplateDto.ReporterName}</span></td>
            </tr>
        </tbody>
    ";

    public static string ChangeStatusIssueContent(ChangeStatusIssueEmailContentDto changeStatusIssueEmailContentDto) => $@"
        <tbody style=""border-bottom:0"">
             <tr>
              <td rowspan=""2"" style=""padding:0;padding:0;padding-top:14px;vertical-align:top;width:40px"" width=""40""><img src=""https://ci6.googleusercontent.com/proxy/E6XBKLyCIitch5qhKUwq0Axyl6u1MMA3CsblNCaTJIij-lsfCrBB5I4YPFRhOvTdLtc2uimGReT04f6Du3F09yrLWM4y-HeL6FlnA-oYF8XLGj2PF0012cL_mgt0CFi0eY0qqZtruhejbz5tYAeM-BsGbwGjFQdjl_bX0WdmSSYf0LSqH2XT1mV0kNJ6tgr5z7nGG-aUKgTq8bV8c3U488Tj4ADu_tABzzEfCcYLemB9nSXHevsq2K67hQ=s0-d-e1-ft#https://secure.gravatar.com/avatar/f80e164f797f868bd1faab3fcaaacce2?d=https%3A%2F%2Favatar-management--avatars.us-west-2.prod.public.atl-paas.net%2Finitials%2FBB-4.png"" style=""border:0;border:0;border-radius:50.0%;width:32px;height:32px"" width=""32"" height=""32"" class=""CToWUd"" data-bit=""iit""></td>
              <td colspan=""2"" style=""padding:0;padding:0;padding-top:14px;padding-bottom:6px;line-height:20px;vertical-align:middle""><span style=""color:#42526e;font-weight:500;display:inline-block;vertical-align:middle"">{changeStatusIssueEmailContentDto.ReporterName}</span> <span style=""color:#42526e;display:inline-block;vertical-align:middle""> {changeStatusIssueEmailContentDto.IssueCreationTime:hh:mm tt}</span></td>
             </tr>
        </tbody>
        <tbody style=""border-bottom:0"">
             <tr>
              <td rowspan=""2"" style=""padding:0;padding:0"">&nbsp;</td>
             </tr>
             <tr>
              <td style=""padding:0;padding:0;white-space:nowrap;width:1.0%;vertical-align:top;padding-right:5px;line-height:24px"">Status:</td>
              <td style=""padding:0;padding:0;line-height:24px""><span style=""max-width:700px;height:auto;display:inline-block;padding:0 4px;background-color:#ffebe6;text-decoration:line-through"" bgcolor=""#ffebe6"">{changeStatusIssueEmailContentDto.FromStatusName}</span> <img alt=""→"" id=""m_7162377815005187315logo"" src=""https://mail.google.com/mail/u/0?ui=2&amp;ik=7fab67c955&amp;attid=0.2&amp;permmsgid=msg-f:1763385373260554372&amp;th=1878cda77750a084&amp;view=fimg&amp;fur=ip&amp;sz=s0-l75-ft&amp;attbid=ANGjdJ-UMf7vovLcqXm_neBVmIy2RP5aYf6TzezwbCNcvpwQifFUtONqykfHX-gNEask1n8zHX0fFElziZd71eILIyD8tDHH5moA_i2yCbnUsH686g3hI483yG9pqYU&amp;disp=emb"" style=""border:0;height:24px;width:24px;border:0;max-width:700px;height:auto;height:16px;width:16px;vertical-align:middle"" height=""16"" width=""16"" data-image-whitelisted="""" class=""CToWUd"" data-bit=""iit""> <span style=""max-width:700px;height:auto;display:inline-block;padding:0 4px;background-color:#e3fcef"" bgcolor=""#e3fcef"">{changeStatusIssueEmailContentDto.ToStatusName}</span></td>
             </tr>
        </tbody>
    ";

    public static string AddNewCommentIssueContent(AddNewCommentIssueEmailContentDto newCommentIssueEmailContentDto) => $@"
        <tbody style=""border-bottom:0"">
             <tr>
              <td rowspan=""2"" style=""padding:0;padding:0;padding-top:14px;vertical-align:top;width:40px"" width=""40""><img src=""{newCommentIssueEmailContentDto.AvatarUrl}"" style=""border:0;border:0;border-radius:50.0%;width:32px;height:32px"" width=""32"" height=""32"" class=""CToWUd"" data-bit=""iit""></td>
              <td colspan=""2"" style=""padding:0;padding:0;padding-top:14px;padding-bottom:6px;line-height:20px;vertical-align:middle""><span style=""color:#42526e;font-weight:500;display:inline-block;vertical-align:middle"">{newCommentIssueEmailContentDto.ReporterName}</span> <span style=""color:#42526e;display:inline-block;vertical-align:middle""> {newCommentIssueEmailContentDto.IssueCreationTime}</span></td>
             </tr>
             <tr>
              <td colspan=""2"" style=""padding:0;padding:0;line-height:24px"">
               <div>
                <div style=""font-family:-apple-system,BlinkMacSystemFont,'Segoe UI','Roboto','Oxygen','Ubuntu','Fira Sans','Droid Sans','Helvetica Neue',sans-serif;font-size:14px;font-weight:400;line-height:24px;vertical-align:baseline"">
                 <p style=""margin:10px 0 0;margin-top:0;margin:0;padding:0px;margin-bottom:7px;padding-top:7px;line-height:24px;font-size:14px"">{newCommentIssueEmailContentDto.CommentContent}</p>
                </div>
               </div></td>
             </tr>
        </tbody>
    ";

    public static string ChangeSprintIssueContent(ChangeSprintIssueEmailContentDto changeSprintIssueEmailContentDto)
    {
        // From Backlog to Sprint
        if (string.IsNullOrWhiteSpace(changeSprintIssueEmailContentDto.FromSprintName))
        {
            return $@"
                <tbody style=""border-bottom:0"">
                 <tr>
                  <td rowspan=""2"" style=""padding:0;padding:0;padding-top:14px;vertical-align:top;width:40px"" width=""40""><img src=""{changeSprintIssueEmailContentDto.AvatarUrl}"" style=""border:0;border:0;border-radius:50.0%;width:32px;height:32px"" width=""32"" height=""32"" class=""CToWUd"" data-bit=""iit""></td>
                  <td colspan=""2"" style=""padding:0;padding:0;padding-top:14px;padding-bottom:6px;line-height:20px;vertical-align:middle""><span style=""color:#42526e;font-weight:500;display:inline-block;vertical-align:middle"">Bình - 64IT1 Hoàng Bảo</span> <span style=""color:#42526e;display:inline-block;vertical-align:middle""> 4:42&nbsp;PM&nbsp;ICT</span></td>
                 </tr>
                 <tr>
                  <td style=""padding:0;padding:0;white-space:nowrap;width:1.0%;vertical-align:top;padding-right:5px;line-height:24px"">Sprint:</td>
                  <td style=""padding:0;padding:0;line-height:24px""><span style=""max-width:700px;height:auto;display:inline-block;padding:0 4px;background-color:#e3fcef"" bgcolor=""#e3fcef"">{changeSprintIssueEmailContentDto.ToSprintName}</span></td>
                 </tr>
                </tbody>
            ";
        }
        // From Sprint to Backlog
        else if (string.IsNullOrWhiteSpace(changeSprintIssueEmailContentDto.ToSprintName))
        {
            return $@"
                <tbody style=""border-bottom:0"">
                 <tr>
                  <td rowspan=""2"" style=""padding:0;padding:0;padding-top:14px;vertical-align:top;width:40px"" width=""40""><img src=""{changeSprintIssueEmailContentDto.AvatarUrl}"" style=""border:0;border:0;border-radius:50.0%;width:32px;height:32px"" width=""32"" height=""32"" class=""CToWUd"" data-bit=""iit""></td>
                  <td colspan=""2"" style=""padding:0;padding:0;padding-top:14px;padding-bottom:6px;line-height:20px;vertical-align:middle""><span style=""color:#42526e;font-weight:500;display:inline-block;vertical-align:middle"">Bình - 64IT1 Hoàng Bảo</span> <span style=""color:#42526e;display:inline-block;vertical-align:middle""> 4:42&nbsp;PM&nbsp;ICT</span></td>
                 </tr>
                 <tr>
                  <td style=""padding:0;padding:0;white-space:nowrap;width:1.0%;vertical-align:top;padding-right:5px;line-height:24px"">Sprint:</td>
                  <td style=""padding:0;padding:0;line-height:24px""><span style=""max-width:700px;height:auto;display:inline-block;padding:0 4px;background-color:#e3fcef"" bgcolor=""#e3fcef"">{IssueConstants.None_IssueHistoryContent}</span></td>
                 </tr>
                </tbody>
            ";
        }
        // From Sprint to Sprint
        else
        {

            return $@"
                <tbody style=""border-bottom:0"">
                     <tr>
                      <td rowspan=""2"" style=""padding:0;padding:0;padding-top:14px;vertical-align:top;width:40px"" width=""40""><img src=""{changeSprintIssueEmailContentDto.AvatarUrl}"" style=""border:0;border:0;border-radius:50.0%;width:32px;height:32px"" width=""32"" height=""32"" class=""CToWUd"" data-bit=""iit""></td>
                      <td colspan=""2"" style=""padding:0;padding:0;padding-top:14px;padding-bottom:6px;line-height:20px;vertical-align:middle""><span style=""color:#42526e;font-weight:500;display:inline-block;vertical-align:middle"">{changeSprintIssueEmailContentDto.ReporterName}</span> <span style=""color:#42526e;display:inline-block;vertical-align:middle""> {changeSprintIssueEmailContentDto.IssueCreationTime:hh:mm tt}</span></td>
                     </tr>
                     <tr>
                      <td style=""padding:0;padding:0;white-space:nowrap;width:1.0%;vertical-align:top;padding-right:5px;line-height:24px"">Sprint:</td>
                      <td style=""padding:0;padding:0;line-height:24px""><span style=""max-width:700px;height:auto;display:inline-block;padding:0 4px;background-color:#ffebe6;text-decoration:line-through"" bgcolor=""#ffebe6"">{changeSprintIssueEmailContentDto.FromSprintName}</span> <img alt=""→"" id=""m_1134604952309939511logo"" src=""https://mail.google.com/mail/u/0?ui=2&amp;ik=7fab67c955&amp;attid=0.2&amp;permmsgid=msg-f:1783803644718442420&amp;th=18c157f6c07d67b4&amp;view=fimg&amp;fur=ip&amp;sz=s0-l75-ft&amp;attbid=ANGjdJ_eDmmfseGMA7n3w1eZ4TjVNMyqeHh8vk5tKU9pzw8mH9irI6BUG3ujfpqkfAzRN2ZOo4g56R54XewXnkWu7vRlGPkBJ7lJOeA1W6h5fSnMIQkUcQtPDF1JhQ8&amp;disp=emb"" style=""border:0;height:24px;width:24px;border:0;max-width:700px;height:auto;height:16px;width:16px;vertical-align:middle"" height=""16"" width=""16"" data-image-whitelisted="""" class=""CToWUd"" data-bit=""iit""> <span style=""max-width:700px;height:auto;display:inline-block;padding:0 4px;background-color:#e3fcef"" bgcolor=""#e3fcef"">{changeSprintIssueEmailContentDto.ToSprintName}</span></td>
                     </tr>
                </tbody>
        ";
        }
    }

    public static string ChangeNameIssueContent(ChangeNameIssueEmailContentDto changeNameIssueEmailContentDto) => $@"
        <tbody style=""border-bottom:0"">
             <tr>
              <td rowspan=""2"" style=""padding:0;padding:0;padding-top:14px;vertical-align:top;width:40px"" width=""40""><img src=""{changeNameIssueEmailContentDto.AvatarUrl}"" style=""border:0;border:0;border-radius:50.0%;width:32px;height:32px"" width=""32"" height=""32"" class=""CToWUd"" data-bit=""iit""></td>
              <td colspan=""2"" style=""padding:0;padding:0;padding-top:14px;padding-bottom:6px;line-height:20px;vertical-align:middle""><span style=""color:#42526e;font-weight:500;display:inline-block;vertical-align:middle"">{changeNameIssueEmailContentDto.ReporterName}</span> <span style=""color:#42526e;display:inline-block;vertical-align:middle""> {changeNameIssueEmailContentDto.IssueCreationTime:hh:mm tt}</span></td>
             </tr>
             <tr>
              <td style=""padding:0;padding:0;white-space:nowrap;width:1.0%;vertical-align:top;padding-right:5px;line-height:24px"">Summary:</td>
              <td style=""padding:0;padding:0;line-height:24px""><span style=""max-width:700px;height:auto;display:inline-block;padding:0 4px;background-color:#ffebe6;text-decoration:line-through"" bgcolor=""#ffebe6"">{changeNameIssueEmailContentDto.ToName}</span> <img alt=""→"" id=""m_2389510754228572188logo"" src=""https://mail.google.com/mail/u/0?ui=2&amp;ik=7fab67c955&amp;attid=0.2&amp;permmsgid=msg-f:1780562296656041401&amp;th=18b5d3f998a315b9&amp;view=fimg&amp;fur=ip&amp;sz=s0-l75-ft&amp;attbid=ANGjdJ_83rEGANEjvQZ6bN6y1rIvrwLksRIgj6lO91ban6eDJ7JJcZjmguKGwNBG3d5xIJ1ggLdlgMjpSRZuVIUb5P8_vsBGEXLcd6TvPu0GDfjlUWW6b8w5SsBZF7Y&amp;disp=emb"" style=""border:0;height:24px;width:24px;border:0;max-width:700px;height:auto;height:16px;width:16px;vertical-align:middle"" height=""16"" width=""16"" data-image-whitelisted="""" class=""CToWUd"" data-bit=""iit""> <span style=""max-width:700px;height:auto;display:inline-block;padding:0 4px;background-color:#e3fcef"" bgcolor=""#e3fcef"">{changeNameIssueEmailContentDto.ToName}</span></td>
             </tr>
        </tbody>
    ";

    public static string ChangeDueDateIssueContent(ChangeDueDateIssueEmailContentDto changeDueDateIssueEmailContentDto) => $@"
        <tbody style=""border-bottom:0"">
             <tr>
              <td rowspan=""2"" style=""padding:0;padding:0;padding-top:14px;vertical-align:top;width:40px"" width=""40""><img src=""{changeDueDateIssueEmailContentDto.AvatarUrl}"" style=""border:0;border:0;border-radius:50.0%;width:32px;height:32px"" width=""32"" height=""32"" class=""CToWUd"" data-bit=""iit""></td>
              <td colspan=""2"" style=""padding:0;padding:0;padding-top:14px;padding-bottom:6px;line-height:20px;vertical-align:middle""><span style=""color:#42526e;font-weight:500;display:inline-block;vertical-align:middle"">{changeDueDateIssueEmailContentDto.ReporterName}</span> <span style=""color:#42526e;display:inline-block;vertical-align:middle""> {changeDueDateIssueEmailContentDto.IssueCreationTime}</span></td>
             </tr>
             <tr>
              <td style=""padding:0;padding:0;white-space:nowrap;width:1.0%;vertical-align:top;padding-right:5px;line-height:24px"">Due date:</td>
              <td style=""padding:0;padding:0;line-height:24px""><span style=""max-width:700px;height:auto;display:inline-block;padding:0 4px;background-color:#ffebe6;text-decoration:line-through"" bgcolor=""#ffebe6"">{changeDueDateIssueEmailContentDto.FromDueDate}</span> <img alt=""→"" id=""m_7162377815005187315logo"" src=""https://mail.google.com/mail/u/0?ui=2&amp;ik=7fab67c955&amp;attid=0.2&amp;permmsgid=msg-f:1763385373260554372&amp;th=1878cda77750a084&amp;view=fimg&amp;fur=ip&amp;sz=s0-l75-ft&amp;attbid=ANGjdJ-UMf7vovLcqXm_neBVmIy2RP5aYf6TzezwbCNcvpwQifFUtONqykfHX-gNEask1n8zHX0fFElziZd71eILIyD8tDHH5moA_i2yCbnUsH686g3hI483yG9pqYU&amp;disp=emb"" style=""border:0;height:24px;width:24px;border:0;max-width:700px;height:auto;height:16px;width:16px;vertical-align:middle"" height=""16"" width=""16"" data-image-whitelisted="""" class=""CToWUd"" data-bit=""iit""> <span style=""max-width:700px;height:auto;display:inline-block;padding:0 4px;background-color:#e3fcef"" bgcolor=""#e3fcef"">{changeDueDateIssueEmailContentDto.ToDueDate}</span></td>
             </tr>
        </tbody>
    ";

    public static string ChangeStartDateIssueContent(ChangeStartDateIssueEmailContentDto changeStartDateIssueEmailContentDto) => $@"
        <tbody style=""border-bottom:0"">
             <tr>
              <td rowspan=""2"" style=""padding:0;padding:0;padding-top:14px;vertical-align:top;width:40px"" width=""40""><img src=""{changeStartDateIssueEmailContentDto.AvatarUrl}"" style=""border:0;border:0;border-radius:50.0%;width:32px;height:32px"" width=""32"" height=""32"" class=""CToWUd"" data-bit=""iit""></td>
              <td colspan=""2"" style=""padding:0;padding:0;padding-top:14px;padding-bottom:6px;line-height:20px;vertical-align:middle""><span style=""color:#42526e;font-weight:500;display:inline-block;vertical-align:middle"">{changeStartDateIssueEmailContentDto.ReporterName}</span> <span style=""color:#42526e;display:inline-block;vertical-align:middle""> {changeStartDateIssueEmailContentDto.IssueCreationTime}</span></td>
             </tr>
             <tr>
              <td style=""padding:0;padding:0;white-space:nowrap;width:1.0%;vertical-align:top;padding-right:5px;line-height:24px"">Start date:</td>
              <td style=""padding:0;padding:0;line-height:24px""><span style=""max-width:700px;height:auto;display:inline-block;padding:0 4px;background-color:#ffebe6;text-decoration:line-through"" bgcolor=""#ffebe6"">{changeStartDateIssueEmailContentDto.FromStartDate}</span> <img alt=""→"" id=""m_7162377815005187315logo"" src=""https://mail.google.com/mail/u/0?ui=2&amp;ik=7fab67c955&amp;attid=0.2&amp;permmsgid=msg-f:1763385373260554372&amp;th=1878cda77750a084&amp;view=fimg&amp;fur=ip&amp;sz=s0-l75-ft&amp;attbid=ANGjdJ-UMf7vovLcqXm_neBVmIy2RP5aYf6TzezwbCNcvpwQifFUtONqykfHX-gNEask1n8zHX0fFElziZd71eILIyD8tDHH5moA_i2yCbnUsH686g3hI483yG9pqYU&amp;disp=emb"" style=""border:0;height:24px;width:24px;border:0;max-width:700px;height:auto;height:16px;width:16px;vertical-align:middle"" height=""16"" width=""16"" data-image-whitelisted="""" class=""CToWUd"" data-bit=""iit""> <span style=""max-width:700px;height:auto;display:inline-block;padding:0 4px;background-color:#e3fcef"" bgcolor=""#e3fcef"">{changeStartDateIssueEmailContentDto.ToStartDate}</span></td>
             </tr>
        </tbody>
    ";

    public static string ChangeAssigneeIssueContent(ChangeAssigneeIssueEmailContentDto changeAssigneeIssueEmailContentDto) => $@"
        <tbody style=""border-bottom:0"">
             <tr>
              <td rowspan=""2"" style=""padding:0;padding:0;padding-top:14px;vertical-align:top;width:40px"" width=""40""><img src=""{changeAssigneeIssueEmailContentDto.AvatarUrl}"" style=""border:0;border:0;border-radius:50.0%;width:32px;height:32px"" width=""32"" height=""32"" class=""CToWUd"" data-bit=""iit""></td>
              <td colspan=""2"" style=""padding:0;padding:0;padding-top:14px;padding-bottom:6px;line-height:20px;vertical-align:middle""><span style=""color:#42526e;font-weight:500;display:inline-block;vertical-align:middle"">{changeAssigneeIssueEmailContentDto.ReporterName}</span> <span style=""color:#42526e;display:inline-block;vertical-align:middle""> {changeAssigneeIssueEmailContentDto.IssueCreationTime:hh:mm tt}</span></td>
             </tr>
             <tr>
              <td style=""padding:0;padding:0;white-space:nowrap;width:1.0%;vertical-align:top;padding-right:5px;line-height:24px"">Assignee:</td>
              <td style=""padding:0;padding:0;line-height:24px""><span style=""max-width:700px;height:auto;display:inline-block;padding:0 4px;background-color:#ffebe6;text-decoration:line-through"" bgcolor=""#ffebe6"">{changeAssigneeIssueEmailContentDto.FromAssigneeName}</span> <img alt=""→"" id=""m_-7751645500041868386logo"" src=""https://mail.google.com/mail/u/0?ui=2&amp;ik=7fab67c955&amp;attid=0.2&amp;permmsgid=msg-f:1780701600558421155&amp;th=18b652abd04114a3&amp;view=fimg&amp;fur=ip&amp;sz=s0-l75-ft&amp;attbid=ANGjdJ_N4vFtiWJgtcZ89bVvq5tMzecf67Ig952YD00ODiw9ueHZBt-xiEuz7KlhxaUuJyJvyGa3M6V729mo-ZvuaACzJwkWpgmVQMuNvn9Q5rOj5-_lpx_fZVncsZI&amp;disp=emb"" style=""border:0;height:24px;width:24px;border:0;max-width:700px;height:auto;height:16px;width:16px;vertical-align:middle"" height=""16"" width=""16"" data-image-whitelisted="""" class=""CToWUd"" data-bit=""iit""> <span style=""max-width:700px;height:auto;display:inline-block;padding:0 4px;background-color:#e3fcef"" bgcolor=""#e3fcef"">{changeAssigneeIssueEmailContentDto.ToAssigneeName}</span></td>
             </tr>
        </tbody>
    ";

    public static string ChangeReporterIssueContent(ChangeReporterIssueEmailContentDto changeReporterIssueEmailContentDto) => $@"
        <tbody style=""border-bottom:0"">
             <tr>
              <td rowspan=""2"" style=""padding:0;padding:0;padding-top:14px;vertical-align:top;width:40px"" width=""40""><img src=""{changeReporterIssueEmailContentDto.AvatarUrl}"" style=""border:0;border:0;border-radius:50.0%;width:32px;height:32px"" width=""32"" height=""32"" class=""CToWUd"" data-bit=""iit""></td>
              <td colspan=""2"" style=""padding:0;padding:0;padding-top:14px;padding-bottom:6px;line-height:20px;vertical-align:middle""><span style=""color:#42526e;font-weight:500;display:inline-block;vertical-align:middle"">{changeReporterIssueEmailContentDto.ReporterName}</span> <span style=""color:#42526e;display:inline-block;vertical-align:middle""> {changeReporterIssueEmailContentDto.IssueCreationTime:hh:mm tt}</span></td>
             </tr>
             <tr>
              <td style=""padding:0;padding:0;white-space:nowrap;width:1.0%;vertical-align:top;padding-right:5px;line-height:24px"">Reporter:</td>
              <td style=""padding:0;padding:0;line-height:24px""><span style=""max-width:700px;height:auto;display:inline-block;padding:0 4px;background-color:#ffebe6;text-decoration:line-through"" bgcolor=""#ffebe6"">{changeReporterIssueEmailContentDto.FromReporterName}</span> <img alt=""→"" id=""m_-7751645500041868386logo"" src=""https://mail.google.com/mail/u/0?ui=2&amp;ik=7fab67c955&amp;attid=0.2&amp;permmsgid=msg-f:1780701600558421155&amp;th=18b652abd04114a3&amp;view=fimg&amp;fur=ip&amp;sz=s0-l75-ft&amp;attbid=ANGjdJ_N4vFtiWJgtcZ89bVvq5tMzecf67Ig952YD00ODiw9ueHZBt-xiEuz7KlhxaUuJyJvyGa3M6V729mo-ZvuaACzJwkWpgmVQMuNvn9Q5rOj5-_lpx_fZVncsZI&amp;disp=emb"" style=""border:0;height:24px;width:24px;border:0;max-width:700px;height:auto;height:16px;width:16px;vertical-align:middle"" height=""16"" width=""16"" data-image-whitelisted="""" class=""CToWUd"" data-bit=""iit""> <span style=""max-width:700px;height:auto;display:inline-block;padding:0 4px;background-color:#e3fcef"" bgcolor=""#e3fcef"">{changeReporterIssueEmailContentDto.ToReporterName}</span></td>
             </tr>
        </tbody>
    ";

    public static string ChangeParentIssueContent(ChangeParentIssueEmailContentDto changeParentIssueEmailContentDto) => $@"
        <tbody style=""border-bottom:0"">
             <tr>
              <td rowspan=""2"" style=""padding:0;padding:0;padding-top:14px;vertical-align:top;width:40px"" width=""40""><img src=""{changeParentIssueEmailContentDto.AvatarUrl}"" style=""border:0;border:0;border-radius:50.0%;width:32px;height:32px"" width=""32"" height=""32"" class=""CToWUd"" data-bit=""iit""></td>
              <td colspan=""2"" style=""padding:0;padding:0;padding-top:14px;padding-bottom:6px;line-height:20px;vertical-align:middle""><span style=""color:#42526e;font-weight:500;display:inline-block;vertical-align:middle"">{changeParentIssueEmailContentDto.ReporterName}</span> <span style=""color:#42526e;display:inline-block;vertical-align:middle""> {changeParentIssueEmailContentDto.IssueCreationTime:hh:mm tt}</span></td>
             </tr>
             <tr>
              <td style=""padding:0;padding:0;white-space:nowrap;width:1.0%;vertical-align:top;padding-right:5px;line-height:24px"">Parent:</td>
              <td style=""padding:0;padding:0;line-height:24px""><span style=""max-width:700px;height:auto;display:inline-block;padding:0 4px;background-color:#ffebe6;text-decoration:line-through"" bgcolor=""#ffebe6"">{changeParentIssueEmailContentDto.FromParentName}</span> <img alt=""→"" id=""m_-7751645500041868386logo"" src=""https://mail.google.com/mail/u/0?ui=2&amp;ik=7fab67c955&amp;attid=0.2&amp;permmsgid=msg-f:1780701600558421155&amp;th=18b652abd04114a3&amp;view=fimg&amp;fur=ip&amp;sz=s0-l75-ft&amp;attbid=ANGjdJ_N4vFtiWJgtcZ89bVvq5tMzecf67Ig952YD00ODiw9ueHZBt-xiEuz7KlhxaUuJyJvyGa3M6V729mo-ZvuaACzJwkWpgmVQMuNvn9Q5rOj5-_lpx_fZVncsZI&amp;disp=emb"" style=""border:0;height:24px;width:24px;border:0;max-width:700px;height:auto;height:16px;width:16px;vertical-align:middle"" height=""16"" width=""16"" data-image-whitelisted="""" class=""CToWUd"" data-bit=""iit""> <span style=""max-width:700px;height:auto;display:inline-block;padding:0 4px;background-color:#e3fcef"" bgcolor=""#e3fcef"">{changeParentIssueEmailContentDto.ToParentName}</span></td>
             </tr>
        </tbody>
    ";

    public static string ChangeIssueTypeIssueContent(ChangeIssueTypeIssueEmailContentDto changeIssueTypeIssueEmailContentDto) => $@"
        <tbody style=""border-bottom:0"">
             <tr>
              <td rowspan=""2"" style=""padding:0;padding:0;padding-top:14px;vertical-align:top;width:40px"" width=""40""><img src=""{changeIssueTypeIssueEmailContentDto.AvatarUrl}"" style=""border:0;border:0;border-radius:50.0%;width:32px;height:32px"" width=""32"" height=""32"" class=""CToWUd"" data-bit=""iit""></td>
              <td colspan=""2"" style=""padding:0;padding:0;padding-top:14px;padding-bottom:6px;line-height:20px;vertical-align:middle""><span style=""color:#42526e;font-weight:500;display:inline-block;vertical-align:middle"">{changeIssueTypeIssueEmailContentDto.ReporterName}</span> <span style=""color:#42526e;display:inline-block;vertical-align:middle""> {changeIssueTypeIssueEmailContentDto.IssueCreationTime:hh:mm tt}</span></td>
             </tr>
             <tr>
              <td style=""padding:0;padding:0;white-space:nowrap;width:1.0%;vertical-align:top;padding-right:5px;line-height:24px"">IssueType:</td>
              <td style=""padding:0;padding:0;line-height:24px""><span style=""max-width:700px;height:auto;display:inline-block;padding:0 4px;background-color:#ffebe6;text-decoration:line-through"" bgcolor=""#ffebe6"">{changeIssueTypeIssueEmailContentDto.FromIssueTypeName}</span> <img alt=""→"" id=""m_-7751645500041868386logo"" src=""https://mail.google.com/mail/u/0?ui=2&amp;ik=7fab67c955&amp;attid=0.2&amp;permmsgid=msg-f:1780701600558421155&amp;th=18b652abd04114a3&amp;view=fimg&amp;fur=ip&amp;sz=s0-l75-ft&amp;attbid=ANGjdJ_N4vFtiWJgtcZ89bVvq5tMzecf67Ig952YD00ODiw9ueHZBt-xiEuz7KlhxaUuJyJvyGa3M6V729mo-ZvuaACzJwkWpgmVQMuNvn9Q5rOj5-_lpx_fZVncsZI&amp;disp=emb"" style=""border:0;height:24px;width:24px;border:0;max-width:700px;height:auto;height:16px;width:16px;vertical-align:middle"" height=""16"" width=""16"" data-image-whitelisted="""" class=""CToWUd"" data-bit=""iit""> <span style=""max-width:700px;height:auto;display:inline-block;padding:0 4px;background-color:#e3fcef"" bgcolor=""#e3fcef"">{changeIssueTypeIssueEmailContentDto.ToIssueTypeName}</span></td>
             </tr>
        </tbody>
    ";

    public static string ChangePriorityIssueContent(ChangePriorityIssueEmailContentDto changePriorityIssueEmailContentDto) => $@"
        <tbody style=""border-bottom:0"">
             <tr>
              <td rowspan=""2"" style=""padding:0;padding:0;padding-top:14px;vertical-align:top;width:40px"" width=""40""><img src=""{changePriorityIssueEmailContentDto.AvatarUrl}"" style=""border:0;border:0;border-radius:50.0%;width:32px;height:32px"" width=""32"" height=""32"" class=""CToWUd"" data-bit=""iit""></td>
              <td colspan=""2"" style=""padding:0;padding:0;padding-top:14px;padding-bottom:6px;line-height:20px;vertical-align:middle""><span style=""color:#42526e;font-weight:500;display:inline-block;vertical-align:middle"">{changePriorityIssueEmailContentDto.ReporterName}</span> <span style=""color:#42526e;display:inline-block;vertical-align:middle""> {changePriorityIssueEmailContentDto.IssueCreationTime:hh:mm tt}</span></td>
             </tr>
             <tr>
              <td style=""padding:0;padding:0;white-space:nowrap;width:1.0%;vertical-align:top;padding-right:5px;line-height:24px"">IssueType:</td>
              <td style=""padding:0;padding:0;line-height:24px""><span style=""max-width:700px;height:auto;display:inline-block;padding:0 4px;background-color:#ffebe6;text-decoration:line-through"" bgcolor=""#ffebe6"">{changePriorityIssueEmailContentDto.FromPriorityName}</span> <img alt=""→"" id=""m_-7751645500041868386logo"" src=""https://mail.google.com/mail/u/0?ui=2&amp;ik=7fab67c955&amp;attid=0.2&amp;permmsgid=msg-f:1780701600558421155&amp;th=18b652abd04114a3&amp;view=fimg&amp;fur=ip&amp;sz=s0-l75-ft&amp;attbid=ANGjdJ_N4vFtiWJgtcZ89bVvq5tMzecf67Ig952YD00ODiw9ueHZBt-xiEuz7KlhxaUuJyJvyGa3M6V729mo-ZvuaACzJwkWpgmVQMuNvn9Q5rOj5-_lpx_fZVncsZI&amp;disp=emb"" style=""border:0;height:24px;width:24px;border:0;max-width:700px;height:auto;height:16px;width:16px;vertical-align:middle"" height=""16"" width=""16"" data-image-whitelisted="""" class=""CToWUd"" data-bit=""iit""> <span style=""max-width:700px;height:auto;display:inline-block;padding:0 4px;background-color:#e3fcef"" bgcolor=""#e3fcef"">{changePriorityIssueEmailContentDto.ToPriorityName}</span></td>
             </tr>
        </tbody>
    ";

    public static string ChangeSPEIssueContent(ChangeSPEIssueEmailContentDto changeSPEIssueEmailContentDto) => $@"
        <tbody style=""border-bottom:0"">
             <tr>
              <td rowspan=""2"" style=""padding:0;padding:0;padding-top:14px;vertical-align:top;width:40px"" width=""40""><img src=""{changeSPEIssueEmailContentDto.AvatarUrl}"" style=""border:0;border:0;border-radius:50.0%;width:32px;height:32px"" width=""32"" height=""32"" class=""CToWUd"" data-bit=""iit""></td>
              <td colspan=""2"" style=""padding:0;padding:0;padding-top:14px;padding-bottom:6px;line-height:20px;vertical-align:middle""><span style=""color:#42526e;font-weight:500;display:inline-block;vertical-align:middle"">{changeSPEIssueEmailContentDto.ReporterName}</span> <span style=""color:#42526e;display:inline-block;vertical-align:middle""> {changeSPEIssueEmailContentDto.IssueCreationTime:hh:mm tt}</span></td>
             </tr>
             <tr>
              <td style=""padding:0;padding:0;white-space:nowrap;width:1.0%;vertical-align:top;padding-right:5px;line-height:24px"">IssueType:</td>
              <td style=""padding:0;padding:0;line-height:24px""><span style=""max-width:700px;height:auto;display:inline-block;padding:0 4px;background-color:#ffebe6;text-decoration:line-through"" bgcolor=""#ffebe6"">{changeSPEIssueEmailContentDto.FromSPEName}</span> <img alt=""→"" id=""m_-7751645500041868386logo"" src=""https://mail.google.com/mail/u/0?ui=2&amp;ik=7fab67c955&amp;attid=0.2&amp;permmsgid=msg-f:1780701600558421155&amp;th=18b652abd04114a3&amp;view=fimg&amp;fur=ip&amp;sz=s0-l75-ft&amp;attbid=ANGjdJ_N4vFtiWJgtcZ89bVvq5tMzecf67Ig952YD00ODiw9ueHZBt-xiEuz7KlhxaUuJyJvyGa3M6V729mo-ZvuaACzJwkWpgmVQMuNvn9Q5rOj5-_lpx_fZVncsZI&amp;disp=emb"" style=""border:0;height:24px;width:24px;border:0;max-width:700px;height:auto;height:16px;width:16px;vertical-align:middle"" height=""16"" width=""16"" data-image-whitelisted="""" class=""CToWUd"" data-bit=""iit""> <span style=""max-width:700px;height:auto;display:inline-block;padding:0 4px;background-color:#e3fcef"" bgcolor=""#e3fcef"">{changeSPEIssueEmailContentDto.ToSPEName}</span></td>
             </tr>
        </tbody>
    ";

    public static string AddNewAttachmentIssueContent(AddNewAttachmentIssueEmailContentDto addNewAttachmentIssueEmailContentDto) => $@"
        <tbody style=""border-bottom:0"">
             <tr>
              <td rowspan=""2"" style=""padding:0;padding:0;padding-top:14px;vertical-align:top;width:40px"" width=""40""><img src=""{addNewAttachmentIssueEmailContentDto.AvatarUrl}"" style=""border:0;border:0;border-radius:50.0%;width:32px;height:32px"" width=""32"" height=""32"" class=""CToWUd"" data-bit=""iit""></td>
              <td colspan=""2"" style=""padding:0;padding:0;padding-top:14px;padding-bottom:6px;line-height:20px;vertical-align:middle""><span style=""color:#42526e;font-weight:500;display:inline-block;vertical-align:middle"">{addNewAttachmentIssueEmailContentDto.ReporterName}</span> <span style=""color:#42526e;display:inline-block;vertical-align:middle""> {addNewAttachmentIssueEmailContentDto.IssueCreationTime}</span></td>
             </tr>
             <tr>
              <td colspan=""2"" style=""padding:0;padding:0;line-height:24px"">
               <div>
                <div style=""font-family:-apple-system,BlinkMacSystemFont,'Segoe UI','Roboto','Oxygen','Ubuntu','Fira Sans','Droid Sans','Helvetica Neue',sans-serif;font-size:14px;font-weight:400;line-height:24px;vertical-align:baseline"">
                 <p style=""margin:10px 0 0;margin-top:0;margin:0;padding:0px;margin-bottom:7px;padding-top:7px;line-height:24px;font-size:14px"">{addNewAttachmentIssueEmailContentDto.AttachmentName}</p>
                </div>
               </div></td>
             </tr>
        </tbody>
    ";

    public static string DeleteNewAttachmentIssueContent(DeleteNewAttachmentIssueEmailContentDto deleteNewAttachmentIssueEmailContentDto) => $@"
        <tbody style=""border-bottom:0"">
             <tr>
              <td rowspan=""2"" style=""padding:0;padding:0;padding-top:14px;vertical-align:top;width:40px"" width=""40""><img src=""{deleteNewAttachmentIssueEmailContentDto.AvatarUrl}"" style=""border:0;border:0;border-radius:50.0%;width:32px;height:32px"" width=""32"" height=""32"" class=""CToWUd"" data-bit=""iit""></td>
              <td colspan=""2"" style=""padding:0;padding:0;padding-top:14px;padding-bottom:6px;line-height:20px;vertical-align:middle""><span style=""color:#42526e;font-weight:500;display:inline-block;vertical-align:middle"">{deleteNewAttachmentIssueEmailContentDto.ReporterName}</span> <span style=""color:#42526e;display:inline-block;vertical-align:middle""> {deleteNewAttachmentIssueEmailContentDto.IssueCreationTime}</span></td>
             </tr>
             <tr>
              <td colspan=""2"" style=""padding:0;padding:0;line-height:24px"">
               <div>
                <div style=""font-family:-apple-system,BlinkMacSystemFont,'Segoe UI','Roboto','Oxygen','Ubuntu','Fira Sans','Droid Sans','Helvetica Neue',sans-serif;font-size:14px;font-weight:400;line-height:24px;vertical-align:baseline"">
                 <span style=""max-width:700px;height:auto;display:inline-block;padding:0 4px;background-color:#ffebe6;text-decoration:line-through"" bgcolor=""#ffebe6"">{deleteNewAttachmentIssueEmailContentDto.AttachmentName}</span>
                </div>
               </div></td>
             </tr>
        </tbody>
    ";

    public static string DeleteCommentIssueContent(DeleteCommentIssueEmailContentDto deleteCommentIssueEmailContentDto) => $@"
        <tbody style=""border-bottom:0"">
             <tr>
              <td rowspan=""2"" style=""padding:0;padding:0;padding-top:14px;vertical-align:top;width:40px"" width=""40""><img src=""{deleteCommentIssueEmailContentDto.AvatarUrl}"" style=""border:0;border:0;border-radius:50.0%;width:32px;height:32px"" width=""32"" height=""32"" class=""CToWUd"" data-bit=""iit""></td>
              <td colspan=""2"" style=""padding:0;padding:0;padding-top:14px;padding-bottom:6px;line-height:20px;vertical-align:middle""><span style=""color:#42526e;font-weight:500;display:inline-block;vertical-align:middle"">{deleteCommentIssueEmailContentDto.ReporterName}</span> <span style=""color:#42526e;display:inline-block;vertical-align:middle""> {deleteCommentIssueEmailContentDto.IssueCreationTime}</span></td>
             </tr>
             <tr>
              <td colspan=""2"" style=""padding:0;padding:0;line-height:24px"">
               <div>
                <div style=""font-family:-apple-system,BlinkMacSystemFont,'Segoe UI','Roboto','Oxygen','Ubuntu','Fira Sans','Droid Sans','Helvetica Neue',sans-serif;font-size:14px;font-weight:400;line-height:24px;vertical-align:baseline"">
                 <span style=""max-width:700px;height:auto;display:inline-block;padding:0 4px;background-color:#ffebe6;text-decoration:line-through"" bgcolor=""#ffebe6"">{deleteCommentIssueEmailContentDto.CommentContent}</span>
                </div>
               </div></td>
             </tr>
        </tbody>
    ";

    public static string DeleteIssueContent(DeletedIssueEmailContentDto deletedIssueEmailContent) => $@"
        <tbody style=""border-bottom:0"">
             <tr>
              <td rowspan=""2"" style=""padding:0;padding:0;padding-top:14px;vertical-align:top;width:40px"" width=""40""><img src=""{deletedIssueEmailContent.AvatarUrl}"" style=""border:0;border:0;border-radius:50.0%;width:32px;height:32px"" width=""32"" height=""32"" class=""CToWUd"" data-bit=""iit""></td>
              <td colspan=""2"" style=""padding:0;padding:0;padding-top:14px;padding-bottom:6px;line-height:20px;vertical-align:middle""><span style=""color:#42526e;font-weight:500;display:inline-block;vertical-align:middle"">{deletedIssueEmailContent.ReporterName}</span> <span style=""color:#42526e;display:inline-block;vertical-align:middle""> {deletedIssueEmailContent.IssueCreationTime:hh:mm tt}</span></td>
             </tr>
             <tr>
              <td style=""padding:0;padding:0;line-height:24px""><span style=""max-width:700px;height:auto;display:inline-block;padding:0 4px;background-color:#ffebe6;text-decoration:line-through"" bgcolor=""#ffebe6"">{deletedIssueEmailContent.IssueName}</span> </td>
             </tr>
        </tbody>
    ";
}
