import Index from '../page/index'
import Add from '../components/add'
import Edit from '../components/edit'
import Detail from '../components/detail'
import TestForm from '../page/testForm'
export default [
  {
    path: '/',
    name: 'index',
    component: Index,
    children: [
      {
        path: '/add',
        name: 'add',
        component: Add
      },
      {
        path: '/edit',
        name: 'edit',
        component: Edit,
        props: true
      },
      {
        path: '/detail',
        name: 'detail',
        component: Detail,
        props: true
      }
    ]
  }, {
    path: '/testForm',
    component: TestForm
  },
]