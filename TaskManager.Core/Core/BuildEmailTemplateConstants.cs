namespace TaskManager.Core.Core;

public static class BuildEmailTemplateConstants
{
    public static string BuildEmailTemplate(BuidEmailTemplateBaseDto buidEmailTemplateDto)
    {
        return $@"
        <div style=""width:100.0%;table-layout:fixed;max-width:1040px;margin:0 auto"">
        <table style=""border-spacing:0;margin:0;border-collapse:collapse;width:100.0%;border-spacing:0;margin:0"">
            <tbody style=""border-bottom:0"">
                <tr>
                    <td style=""padding:0;padding:0"">
                        <div style=""font-size:14px;margin-bottom:7px"">
                            <span class=""m_-5525498053404099447summary-text-part"" style=""display:inline"">{buidEmailTemplateDto.SenderName} <b style=""font-weight:600"">{buidEmailTemplateDto.ActionName}</b>.</span>
                        </div>
                        <table
                            style=""border-spacing:0;margin:0;border-collapse:collapse;width:100.0%;border-spacing:0;margin:0;max-width:1040px;margin:0;border-collapse:separate"">
                            <tbody style=""border-bottom:0"">
                                <tr>
                                    <td
                                        style=""padding:0;padding:0;padding-top:12px;padding-bottom:12px;border-top:2px solid #dfe1e6;border-bottom:2px solid #dfe1e6"">
                                        <table
                                            style=""border-spacing:0;margin:0;border-collapse:collapse;width:100.0%;border-spacing:0;margin:0;margin:0 0 0 0"">
                                            <thead style=""border-bottom:0"">
                                                <tr>
                                                    <td colspan=""3"" style=""padding:0;padding:0"">
                                                        <div>
                                                            <div>
                                                                <div
                                                                    style=""color:#6b778c;font-weight:500;font-size:14px;line-height:20px;margin-bottom:6px"">
                                                                    <table
                                                                        style=""border-spacing:0;margin:0;border-collapse:collapse;width:100.0%;border-spacing:0;margin:0;width:auto;border-spacing:0;height:20px""
                                                                        height=""20"">
                                                                        <tbody style=""border-bottom:0"">
                                                                            <tr>
                                                                                <td
                                                                                    style=""padding:0;padding:0;white-space:nowrap;color:#6b778c"">
                                                                                    <table
                                                                                        style=""border-spacing:0;margin:0;border-collapse:collapse;width:100.0%;border-spacing:0;margin:0"">
                                                                                        <tbody style=""border-bottom:0"">
                                                                                            <tr>
                                                                                                <td
                                                                                                    style=""padding:0;padding:0;white-space:nowrap;color:#6b778c"">
                                                                                                    <a class=""m_-5525498053404099447ak-button__appearance-subtle-link""
                                                                                                        href=""{buidEmailTemplateDto.ProjectLink}""
                                                                                                        style=""color:#0052cc;text-decoration:none;box-sizing:border-box;border-radius:3px;border-width:0;display:inline-flex;font-style:normal;font-size:inherit;height:2.2857144em;line-height:2.2857144em;margin:0;outline:none;padding:0 12px;text-align:center;vertical-align:middle;white-space:nowrap;background:none;color:#42526e;text-decoration:none;padding:0px;color:#6b778c;height:auto;line-height:normal""
                                                                                                        target=""_blank""
                                                                                                        data-saferedirecturl=""https://www.google.com/url?q={buidEmailTemplateDto.ProjectLink};source=gmail&amp;ust=1701759535684000&amp;usg=AOvVaw2jTbsvkHJg0pgz8Fh6ouS9"">{buidEmailTemplateDto.ProjectName}</a>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </tbody>
                                                                                    </table>
                                                                                </td>
                                                                                <td
                                                                                    style=""padding:0;padding:0;white-space:nowrap;color:#6b778c"">
                                                                                    <span
                                                                                        style=""color:#6b778c"">&nbsp;&nbsp;/&nbsp;&nbsp;</span>
                                                                                </td>
                                                                                <td
                                                                                    style=""padding:0;padding:0;white-space:nowrap;color:#6b778c"">
                                                                                    <table
                                                                                        style=""border-spacing:0;margin:0;border-collapse:collapse;width:100.0%;border-spacing:0;margin:0"">
                                                                                        <tbody style=""border-bottom:0"">
                                                                                            <tr>
                                                                                                <td
                                                                                                    style=""padding:0;padding:0;white-space:nowrap;color:#6b778c"">
                                                                                                    <a class=""m_-5525498053404099447ak-button__appearance-subtle-link""
                                                                                                        href=""{buidEmailTemplateDto.IssueLink}""
                                                                                                        style=""color:#0052cc;text-decoration:none;box-sizing:border-box;border-radius:3px;border-width:0;display:inline-flex;font-style:normal;font-size:inherit;height:2.2857144em;line-height:2.2857144em;margin:0;outline:none;padding:0 12px;text-align:center;vertical-align:middle;white-space:nowrap;background:none;color:#42526e;text-decoration:none;padding:0px;color:#6b778c;height:auto;line-height:normal""
                                                                                                        target=""_blank""
                                                                                                        data-saferedirecturl=""https://www.google.com/url?q={buidEmailTemplateDto.IssueLink};source=gmail&amp;ust=1701759535684000&amp;usg=AOvVaw290j2jPHB-VO2eX48-g1Y7"">{buidEmailTemplateDto.IssueCode}</a>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </tbody>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                        </tbody>
                                                                    </table>
                                                                </div>
                                                            </div>
                                                            <div class=""m_-5525498053404099447issue-summary""
                                                                style=""font-size:20px;font-weight:normal;line-height:24px;margin:6px 0 0 0"">
                                                                <a href=""https://techshopmanagement.atlassian.net/browse/TC-234?atlOrigin=eyJpIjoiMWE5OWQwZTNiNjQ4NDFjYTk3ZDVhM2M1NTFhZjU0MGEiLCJwIjoiaiJ9""
                                                                    style=""color:#0052cc;text-decoration:none;color:#0052cc""
                                                                    target=""_blank""
                                                                    data-saferedirecturl=""https://www.google.com/url?q=https://techshopmanagement.atlassian.net/browse/TC-234?atlOrigin%3DeyJpIjoiMWE5OWQwZTNiNjQ4NDFjYTk3ZDVhM2M1NTFhZjU0MGEiLCJwIjoiaiJ9&amp;source=gmail&amp;ust=1701759535684000&amp;usg=AOvVaw290j2jPHB-VO2eX48-g1Y7"">
                                                                    <span>{buidEmailTemplateDto.IssueName}</span> </a>
                                                            </div>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </thead>
                                            <tbody style=""border-bottom:0"">
                                            </tbody>
                                            {buidEmailTemplateDto.EmailContent}
                                            <tbody style=""border-bottom:0"">
                                                <tr>
                                                    <td colspan=""3"" height=""12px"" style=""padding:0;padding:0"">&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan=""3"" style=""padding:0;padding:0"">
                                                        <div style=""margin:0 0 6px 0"">
                                                            <table
                                                                style=""border-spacing:0;margin:0;border-collapse:collapse;width:100.0%;border-spacing:0;margin:0"">
                                                                <tbody style=""border-bottom:0"">
                                                                    <tr>
                                                                        <td style=""padding:0;padding:0""> <a
                                                                                class=""m_-5525498053404099447ak-button__appearance-primary""
                                                                                href=""{buidEmailTemplateDto.IssueLink}""
                                                                                style=""color:#0052cc;text-decoration:none;box-sizing:border-box;border-radius:3px;border-width:0;display:inline-flex;font-style:normal;font-size:inherit;height:2.2857144em;line-height:2.2857144em;margin:0;outline:none;padding:0 12px;text-align:center;vertical-align:middle;white-space:nowrap;background:#0052cc;color:#ffffff;text-decoration:none;font-family:-apple-system,BlinkMacSystemFont,'Segoe UI','Roboto','Oxygen','Ubuntu','Fira Sans','Droid Sans','Helvetica Neue',sans-serif;font-size:14px;padding:0 38px""
                                                                                target=""_blank""
                                                                                data-saferedirecturl=""https://www.google.com/url?q={buidEmailTemplateDto.IssueLink};source=gmail&amp;ust=1701759535684000&amp;usg=AOvVaw290j2jPHB-VO2eX48-g1Y7"">View issue</a> </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style=""padding:0;padding:0;padding-top:12px"">
                                        <table class=""m_-5525498053404099447footer"" width=""100%"" cellpadding=""0""
                                            cellspacing=""0"" border=""0""
                                            style=""border-spacing:0;margin:0;border-collapse:collapse;width:100.0%;border-spacing:0;margin:0"">
                                            <tbody style=""border-bottom:0"">
                                                <tr>
                                                    <td style=""padding:0;padding:0"">
                                                        <table
                                                            style=""border-spacing:0;margin:0;border-collapse:collapse;width:100.0%;border-spacing:0;margin:0"">
                                                            <tbody style=""border-bottom:0"">
                                                                <tr>
                                                                    <td style=""padding:0;padding:0""><small
                                                                            style=""color:#707070;font-size:12px;line-height:1.3333334;color:#707070;font-size:12px;font-weight:normal;line-height:17px"">Get
                                                                            Jira notifications on your phone! Download
                                                                            the Jira Cloud app for <a
                                                                                href=""https://play.google.com/store/apps/details?id=com.atlassian.android.jira.core&amp;referrer=utm_source%3DNotificationLink%26utm_medium%3DEmail""
                                                                                style=""color:#0052cc;text-decoration:none""
                                                                                target=""_blank""
                                                                                data-saferedirecturl=""https://www.google.com/url?q=https://play.google.com/store/apps/details?id%3Dcom.atlassian.android.jira.core%26referrer%3Dutm_source%253DNotificationLink%2526utm_medium%253DEmail&amp;source=gmail&amp;ust=1701759535684000&amp;usg=AOvVaw2vJtA5-CckrSymL_onite_"">Android</a>
                                                                            or <a
                                                                                href=""https://itunes.apple.com/app/apple-store/id1006972087?pt=696495&amp;ct=EmailNotificationLink&amp;mt=8""
                                                                                style=""color:#0052cc;text-decoration:none""
                                                                                target=""_blank""
                                                                                data-saferedirecturl=""https://www.google.com/url?q=https://itunes.apple.com/app/apple-store/id1006972087?pt%3D696495%26ct%3DEmailNotificationLink%26mt%3D8&amp;source=gmail&amp;ust=1701759535684000&amp;usg=AOvVaw3PdAMw1l8ayNpJtJumwPlV"">iOS</a>.</small>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style=""padding:0;padding:0"">
                                                                        <div style=""line-height:4px;height:4px;font-size:1px""
                                                                            height=""4"">
                                                                            &nbsp;
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style=""padding:0;padding:0;line-height:15px"">
                                                                        <small
                                                                            style=""color:#707070;font-size:12px;line-height:1.3333334;color:#707070;font-size:12px;font-weight:normal;line-height:17px"">
                                                                            <a href=""https://surveys.atlassian.com/jfe/form/SV_9X3zi1X4q1gKsqF""
                                                                                style=""color:#0052cc;text-decoration:none""
                                                                                target=""_blank""
                                                                                data-saferedirecturl=""https://www.google.com/url?q=https://surveys.atlassian.com/jfe/form/SV_9X3zi1X4q1gKsqF&amp;source=gmail&amp;ust=1701759535684000&amp;usg=AOvVaw0JDboaow0RViLwHTVgq1BW"">Give
                                                                                feedback</a> </small> <small
                                                                            style=""color:#707070;font-size:12px;line-height:1.3333334;color:#707070;font-size:12px;font-weight:normal;line-height:17px"">
                                                                            &nbsp;•&nbsp; </small> <small
                                                                            style=""color:#707070;font-size:12px;line-height:1.3333334;color:#707070;font-size:12px;font-weight:normal;line-height:17px"">
                                                                            <a href=""https://www.atlassian.com/legal/privacy-policy""
                                                                                style=""color:#0052cc;text-decoration:none""
                                                                                target=""_blank""
                                                                                data-saferedirecturl=""https://www.google.com/url?q=https://www.atlassian.com/legal/privacy-policy&amp;source=gmail&amp;ust=1701759535684000&amp;usg=AOvVaw3jtK9rpwDeiCMAMdxjVOoN"">Privacy
                                                                                policy</a> </small></td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </td>
                                                    <td width=""20"" class=""m_-5525498053404099447desktop-only""
                                                        style=""padding:0;padding:0;display:block"">&nbsp;</td>
                                                    <td style=""padding:0;padding:0;text-align:right"">
                                                        <div class=""m_-5525498053404099447desktop-only""
                                                            style=""display:block"">
                                                            <img src=""https://mail.google.com/mail/u/0?ui=2&amp;ik=7fab67c955&amp;attid=0.1&amp;permmsgid=msg-f:1783322486121523303&amp;th=18bfa25a493d6867&amp;view=fimg&amp;fur=ip&amp;sz=s0-l75-ft&amp;attbid=ANGjdJ9t4kuUkY6ydS0FgV6DFqFztHpKy2iUTVC9y7I99uobUgB-sfSJ-HKTmNZytm-hTQoZj_btb06Xi5SGkNIpA54j38FFc-MwtCtAILXbdCZfb8pXY0VEVw24K0Q&amp;disp=emb""
                                                                style=""border:0;border:0;height:32px"" height=""32""
                                                                data-image-whitelisted="""" class=""CToWUd"" data-bit=""iit"">
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style=""padding:0;padding:0"">
                                                        <div class=""m_-5525498053404099447mobile-only""
                                                            style=""display:none;line-height:8px;height:8px"" height=""8"">
                                                            &nbsp;
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style=""padding:0;padding:0"">
                                                        <div class=""m_-5525498053404099447mobile-only""
                                                            style=""display:none"">
                                                            <img src=""https://mail.google.com/mail/u/0?ui=2&amp;ik=7fab67c955&amp;attid=0.1&amp;permmsgid=msg-f:1783322486121523303&amp;th=18bfa25a493d6867&amp;view=fimg&amp;fur=ip&amp;sz=s0-l75-ft&amp;attbid=ANGjdJ_-2dcTzM689uXyPCVwhghfo5wkd8tW8ErDH-LGr_WcriSSHNvd6lBMj8cPoBd8k8MZOX9vBlvJZUK-bOELH-0LgXGwqI-9omHeYZNV7uWQWUofkpS98Qw7enA&amp;disp=emb""
                                                                style=""border:0;border:0;height:32px"" height=""32""
                                                                data-image-whitelisted="""" class=""CToWUd"" data-bit=""iit"">
                                                        </div>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    ";
    }
}
