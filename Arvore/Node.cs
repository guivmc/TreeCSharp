using ConsoleApp1.PerguntasTest;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Arvore
{
    public class Node<T> : IEnumerable<Node<T>>
    {
        //Vars
        //Private
        public T Data { get; set; }
        private Node<T> Parent { get; set; }
        private ICollection<Node<T>> Children { get; set; }

        public Node(T data)
        {
            this.Data = data;
            this.Children = new LinkedList<Node<T>>();

            this.ElementsIndex = new LinkedList<Node<T>>();
            this.ElementsIndex.Add(this);
        }

        //Methods
        public Node<T> addChild(T child)
        {
            //Parent of the new child is this node
            Node<T> childNode = new Node<T>(child) { Parent = this };
            //Push new child to the child linked list
            this.Children.Add(childNode);

            this.RegisterChildForSearch(childNode);

            return childNode;
        }

        //Bools
        public Boolean isLeaf
        {
            get { return Children.Count == 0; }
        }

        public Boolean isRoot
        {
            get { return Parent == null; }
        }

        //Ints
        public int level
        {
            get
            {
                if (this.isRoot) return 0;
                //Check all nodes to get the level
                return (Parent.level + 1);
            }
        }

        #region search

        private ICollection<Node<T>> ElementsIndex { get; set; }

        //Push to linked list
        private void RegisterChildForSearch(Node<T> node)
        {
            ElementsIndex.Add(node);

            if (Parent != null) Parent.RegisterChildForSearch(node);
        }

        //Getter
        public Node<T> FindNode(Func<Node<T>, bool> predicate)
        {
            //Get first node that matches
            return this.ElementsIndex.FirstOrDefault(predicate);
        }

        public Node<T>[] getChildrenAsArray(Node<T> parentNode)
        {
            Node<T>[] arr = new Node<T>[parentNode.Children.Count];
            parentNode.Children.CopyTo(arr, 0);
            return arr;
        }

        public Node<T> getChildInChildrenArrayByIndex(Node<T> parentNode, int index)
        {
            Node<T>[] arr = new Node<T>[parentNode.Children.Count];
            parentNode.Children.CopyTo(arr, 0);
            return arr[index];
        }

        #endregion

        #region iterating


        public IEnumerator<Node<T>> GetEnumerator()
        {
            yield return this;
            foreach (var directChild in this.Children)
            {
                foreach (var anyChild in directChild)
                    yield return anyChild;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
