using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace OnlinePharmacy.Models;

public partial class OnlinePharmacyContext : DbContext
{
    public OnlinePharmacyContext()
    {
    }

    public OnlinePharmacyContext(DbContextOptions<OnlinePharmacyContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Blog> Blogs { get; set; }

    public virtual DbSet<BlogTag> BlogTags { get; set; }

    public virtual DbSet<Menu> Menus { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductCategory> ProductCategories { get; set; }

    public virtual DbSet<ProductInventory> ProductInventories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.\\SQLExpress;Database=OnlinePharmacy;Trusted_Connection=True;Encrypt=false");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Blog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tmp_ms_x__3213E83F31B5D3CA");

            entity.ToTable("Blog");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Author)
                .HasMaxLength(100)
                .HasColumnName("author");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.CreateAt)
                .HasColumnType("datetime")
                .HasColumnName("create_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.ModifiedAt)
                .HasColumnType("datetime")
                .HasColumnName("modified_at");
            entity.Property(e => e.Summary).HasColumnName("summary");
            entity.Property(e => e.Tags)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("tags");
            entity.Property(e => e.Thumb)
                .IsUnicode(false)
                .HasColumnName("thumb");
            entity.Property(e => e.Title).HasColumnName("title");
        });

        modelBuilder.Entity<BlogTag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BlogTag__3213E83F97F7E44B");

            entity.ToTable("BlogTag");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Frequency)
                .HasDefaultValueSql("((0))")
                .HasColumnName("frequency");
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Menu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tmp_ms_x__3213E83F2E47EA61");

            entity.ToTable("Menu");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreateAt)
                .HasColumnType("datetime")
                .HasColumnName("create_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .HasColumnName("description");
            entity.Property(e => e.Hide)
                .HasDefaultValueSql("((0))")
                .HasColumnName("hide");
            entity.Property(e => e.Meta)
                .HasMaxLength(50)
                .HasColumnName("meta");
            entity.Property(e => e.ModifiedAt)
                .HasColumnType("datetime")
                .HasColumnName("modified_at");
            entity.Property(e => e.Order).HasColumnName("order");
            entity.Property(e => e.ParentId)
                .HasDefaultValueSql("((0))")
                .HasColumnName("parentId");
            entity.Property(e => e.Title)
                .HasMaxLength(30)
                .HasColumnName("title");
            entity.Property(e => e.Type)
                .HasMaxLength(30)
                .HasColumnName("type");
            entity.Property(e => e.Url)
                .HasDefaultValueSql("('#')")
                .HasColumnName("url");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Product__3213E83F35418ED5");

            entity.ToTable("Product");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CategoryId).HasColumnName("categoryId");
            entity.Property(e => e.CreateAt)
                .HasColumnType("datetime")
                .HasColumnName("create_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.Desc)
                .HasColumnType("text")
                .HasColumnName("desc");
            entity.Property(e => e.Image)
                .IsUnicode(false)
                .HasColumnName("image");
            entity.Property(e => e.ModifiedAt)
                .HasColumnType("datetime")
                .HasColumnName("modified_at");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Price)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("price");
            entity.Property(e => e.Unit)
                .HasMaxLength(20)
                .HasColumnName("unit");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__Product__categor__5DCAEF64");
        });

        modelBuilder.Entity<ProductCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tmp_ms_x__3213E83F8608B469");

            entity.ToTable("ProductCategory");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreateAt)
                .HasColumnType("datetime")
                .HasColumnName("create_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.Desc)
                .HasColumnType("text")
                .HasColumnName("desc");
            entity.Property(e => e.Meta)
                .HasMaxLength(50)
                .HasColumnName("meta");
            entity.Property(e => e.ModifiedAt)
                .HasColumnType("datetime")
                .HasColumnName("modified_at");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.ParentId)
                .HasDefaultValueSql("((0))")
                .HasColumnName("parentId");
        });

        modelBuilder.Entity<ProductInventory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ProductI__3213E83F5653B6D1");

            entity.ToTable("ProductInventory");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CreateAt)
                .HasColumnType("datetime")
                .HasColumnName("create_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.ModifiedAt)
                .HasColumnType("datetime")
                .HasColumnName("modified_at");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.ProductInventory)
                .HasForeignKey<ProductInventory>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductInven__id__2F10007B");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
