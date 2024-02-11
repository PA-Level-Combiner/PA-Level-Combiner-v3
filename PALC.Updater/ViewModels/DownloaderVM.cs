using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

using Octokit;
using Semver;
using System.Collections.Generic;

namespace PALC.Updater.ViewModels;



public class GithubRelease
{
    public required string name;
    public required string url;
    public required SemVersion? version;
}


public partial class DownloaderVM : ViewModelBase
{
    private static readonly GitHubClient client = new(new ProductHeaderValue("PALC-Updater"));


    public ObservableCollection<GithubRelease> GithubReleases { get; set; } = [];


    public event AsyncEventHandler<Exception>? GetReleasesFromRepoFailed;
    public event AsyncEventHandler<Exception>? StringToSemverFailed;

    public event AsyncEventHandler? StartLoad;
    public event AsyncEventHandler? EndLoad;

    public async Task LoadReleases()
    {
        if (StartLoad != null) await StartLoad(this, new EventArgs());
        
        try
        {
            IReadOnlyList<Release> releases;
            try
            {
                releases = await client.Repository.Release.GetAll(Globals.githubID);
            }
            catch (Exception ex)
            {
                if (GetReleasesFromRepoFailed != null)
                    await GetReleasesFromRepoFailed(this, ex);

                return;
            }

            foreach (var release in releases)
            {
                SemVersion? version;
                try
                {
                    version = SemVersion.Parse(release.TagName, SemVersionStyles.Any);
                }
                catch (Exception ex)
                {
                    if (StringToSemverFailed != null)
                        await StringToSemverFailed(this, ex);

                    version = null;
                }

                GithubReleases.Add(new GithubRelease { name = release.Name, url = release.Url, version = version });
            }
        }
        finally
        {
            if (EndLoad != null) await EndLoad(this, new EventArgs());
        }
    }
}
