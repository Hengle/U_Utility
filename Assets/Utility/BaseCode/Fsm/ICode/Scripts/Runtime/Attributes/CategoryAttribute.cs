using System;

namespace ICode{
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class CategoryAttribute : Attribute
	{
		private readonly string category;
		
		public string Category
		{
			get
			{
				return this.category;
			}
		}
		
		public CategoryAttribute(string category)
		{
			this.category = category;
		}
		
		public CategoryAttribute(Category category)
		{   
            if(  category == ICode.Category.GameState )
                this.category = "GameState/" + category.ToString();
            else
                this.category = "UnityAPI/"+ category.ToString();
		}
	}
}