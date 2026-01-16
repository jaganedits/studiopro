import { Routes } from '@angular/router';
import { Login } from './components/start/authentication/login/login';
import { Home } from './components/start/layouts/home/home';
import { LeftMenu } from './components/start/layouts/left-menu/left-menu';

export const routes: Routes = [
    { path: '', redirectTo: '/login', pathMatch: 'full' },
    { path: 'login', component: Login },

    // Error pages
    {
        path: 'unauthorized',
        loadComponent: () =>
            import('./components/shared/error-pages/unauthorized/unauthorized').then(
                (c) => c.Unauthorized
            ),
    },
    {
        path: 'not-found',
        loadComponent: () =>
            import('./components/shared/error-pages/not-found/not-found').then(
                (c) => c.NotFound
            ),
    },
    {
        path: 'home',
        component: LeftMenu,
        // canActivate: [authGuard],
        children: [
            //App routes
             {
                path: 'company',
                // canActivate: [childGuard],
                loadComponent: () =>
                    import(
                        '../app/components/admin/company/company'
                    ).then((c) => c.Company),
                data: {
                    functionid: 15,
                },

            },
            {
                path: 'v-users',
                // canActivate: [childGuard],
                loadComponent: () =>
                    import(
                        '../app/components/admin/users/v-users/v-users'
                    ).then((c) => c.VUsers),
                data: {
                    functionid: 15,
                },

            },
            {
                path: 'c-users',
                // canActivate: [childGuard],
                loadComponent: () =>
                    import(
                        '../app/components/admin/users/c-users/c-users'
                    ).then((c) => c.CUsers),
                data: {
                    functionid: 15,
                },

            },
            {
                path: 'v-role',
                // canActivate: [childGuard],
                loadComponent: () =>
                    import(
                        '../app/components/admin/role/v-role/v-role'
                    ).then((c) => c.VRole),
                data: {
                    functionid: 15,
                },

            },
            {
                path: 'c-role',
                // canActivate: [childGuard],
                loadComponent: () =>
                    import(
                        '../app/components/admin/role/c-role/c-role'
                    ).then((c) => c.CRole),
                data: {
                    functionid: 15,
                },

            },
             {
                path: 'c-branchs',
                // canActivate: [childGuard],
                loadComponent: () =>
                    import(
                        '../app/components/admin/branch/c-branchs/c-branchs'
                    ).then((c) => c.CBranchs),
                data: {
                    functionid: 15,
                },

            },
               {
                path: 'v-branchs',
                // canActivate: [childGuard],
                loadComponent: () =>
                    import(
                        '../app/components/admin/branch/v-branchs/v-branchs'
                    ).then((c) => c.VBranchs),
                data: {
                    functionid: 15,
                },

            },

        ]
    },

    // Wildcard route - MUST be last (catches all unmatched routes)
    { path: '**', redirectTo: '/not-found' },
    
];
