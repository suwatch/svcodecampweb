// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IoC.cs" company="Web Advanced">
// Copyright 2012 Web Advanced (www.webadvanced.com)
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


using System.Collections.Generic;
using StructureMap;
using WebAPI.Code;
using WebAPI.Controllers;
using WebAPI.Repositories;
using WebAPI.ViewModels;

namespace WebAPI.DependencyResolution {
    public static class IoC {
        public static IContainer Initialize() {
            ObjectFactory.Initialize(x =>
                        {
                            x.Scan(scan =>
                                    {
                                        scan.TheCallingAssembly();
                                        scan.WithDefaultConventions();
                                    });

                            if (ControllerUtils.IsTestMode)
                            {
                                x.For<IRepositorySponsor>().Use<SponsorTest>();
                                x.For<IRepositoryPresenter>().Use<PresenterTest>();
                                x.For<IRepositorySession>().Use<SessionTest>();
                            }
                            else
                            {
                                x.For<IRepositorySponsor>().Use<Sponsor>();
                                x.For<IRepositoryPresenter>().Use<Presenter>();
                                x.For<IRepositorySession>().Use<Session>();
                            }
                        }
                        
                        );
            return ObjectFactory.Container;
        }
    }

    
}